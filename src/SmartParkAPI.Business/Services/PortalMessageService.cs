using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartParkAPI.Business.Services.Base;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Contracts.DTO.PortalMessage;
using SmartParkAPI.Contracts.Services;
using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.DataAccess.Interfaces;
using SmartParkAPI.Model.Concrete;

namespace SmartParkAPI.Business.Services
{
    public class PortalMessageService : EntityService<PortalMessageDto, PortalMessage, Guid>, IPortalMessageService
    {
        private readonly IPortalMessageRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public PortalMessageService(IPortalMessageRepository repository, IUnitOfWork unitOfWork, IMapper mapper, IUserRepository userRepository)
            : base(repository, unitOfWork, mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<PortalMessageClustersDto>> GetPortalMessageClusterForCurrentUserAsync(int userId)
        {
            var allUserMessages = (await _repository.GetAllAsync(x => x.UserId == userId || x.ReceiverUserId == userId)).ToList();
            var starterMessages = allUserMessages.Where(x => x.Starter).ToList();

            var currentUser = _mapper.Map<PortalMessageUserDto>(await _userRepository.Include(x => x.UserPreferences).SingleOrDefaultAsync(x => x.Id == userId));

            var result = new PortalMessageClustersDto { User = currentUser, Clusters = new List<PortalMessageClusterDto>() };

            foreach (var starterMessage in starterMessages)
            {
                if ((starterMessage.HiddenForSender && starterMessage.UserId == userId) ||
                    (starterMessage.HiddenForReceiver && starterMessage.ReceiverUserId == userId))
                {
                    continue;
                }

                var receiverUserId = starterMessage.UserId == userId
                    ? starterMessage.ReceiverUserId
                    : starterMessage.UserId;
                var receiverUser = await _userRepository.Include(x => x.UserPreferences).SingleOrDefaultAsync(x => x.Id == receiverUserId);

                var mappedReceiverUser = receiverUser != null
                    ? _mapper.Map<PortalMessageUserDto>(receiverUser)
                    : CreateEmptyPlaceholderForDeletedUser();

                var clusterResult = new PortalMessageClusterDto
                {
                    ReceiverUser = mappedReceiverUser
                };

                var tempStack = new Stack<PortalMessage>();
                tempStack.Push(starterMessage);
                PushToCurrentMessageStack(tempStack, allUserMessages);

                clusterResult.Cluster = tempStack.Select(_mapper.Map<PortalMessageDto>).ToArray();
                result.Clusters.Add(clusterResult);
            }
            result.Clusters = result.Clusters.OrderByDescending(x => x.Cluster[0].CreateDate).ToList();
            return ServiceResult<PortalMessageClustersDto>.Success(result);
        }

        public async Task<ServiceResult> FakeDeleteCluster(int userId, Guid starterMessageId)
        {
            var allUserMessages = (await _repository.GetAllAsync(x => x.UserId == userId || x.ReceiverUserId == userId)).ToList();
            var starterMessage = allUserMessages.SingleOrDefault(x => x.Starter && x.Id == starterMessageId);
            if (starterMessage == null)
            {
                return ServiceResult.Failure("Wystąpił błąd podczas usuwania konwersacji.");
            }
            var tempStack = new Stack<PortalMessage>();
            tempStack.Push(starterMessage);
            PushToCurrentMessageStack(tempStack, allUserMessages);
            var listToEdit = tempStack.ToList();

            foreach (var portalMessage in listToEdit)
            {
                if (userId == portalMessage.ReceiverUserId)
                {
                    portalMessage.HiddenForReceiver = true;
                }
                else if (userId == portalMessage.UserId)
                {
                    portalMessage.HiddenForSender = true;
                }
                else
                {
                    return ServiceResult.Failure("Wystąpił błąd podczas usuwania konwersacji.");
                }
                _repository.Edit(portalMessage);
            }
            await _unitOfWork.CommitAsync();
            return ServiceResult.Success();
        }

        public async Task<ServiceResult<int>> GetUnreadClustersCountAsync(int userId)
        {
            var allUserMessages = (await _repository.GetAllAsync(x => x.UserId == userId || x.ReceiverUserId == userId)).ToList();
            var starterMessages = allUserMessages.Where(x => x.Starter).ToList();

            var result = new List<PortalMessageClusterDto>();

            foreach (var starterMessage in starterMessages)
            {
                if ((starterMessage.HiddenForSender && starterMessage.UserId == userId) ||
                    (starterMessage.HiddenForReceiver && starterMessage.ReceiverUserId == userId))
                {
                    continue;
                }

                var receiverUserId = starterMessage.UserId == userId
                    ? starterMessage.ReceiverUserId
                    : starterMessage.UserId;
                var receiverUser = await _userRepository.Include(x => x.UserPreferences).SingleOrDefaultAsync(x => x.Id == receiverUserId);

                var mappedReceiverUser = receiverUser != null
                    ? _mapper.Map<PortalMessageUserDto>(receiverUser)
                    : CreateEmptyPlaceholderForDeletedUser();

                var clusterResult = new PortalMessageClusterDto
                {
                    ReceiverUser = mappedReceiverUser
                };

                var tempStack = new Stack<PortalMessage>();
                tempStack.Push(starterMessage);
                PushToCurrentMessageStack(tempStack, allUserMessages);

                clusterResult.Cluster = tempStack.Select(_mapper.Map<PortalMessageDto>).ToArray();
                result.Add(clusterResult);
            }

            var count = result.Count(portalMessageClusterDto => portalMessageClusterDto.Cluster[0].ReceiverUserId == userId && !portalMessageClusterDto.Cluster[0].IsDisplayed);
            return ServiceResult<int>.Success(count);
        }

        public async Task<ServiceResult> FakeDelete(Guid messageId, int userId)
        {
            var message = await _repository.FindAsync(messageId);

            var hideForReceiverUser = userId == message.ReceiverUserId;
            var hideForUser = userId == message.UserId;
            if (hideForReceiverUser)
            {
                message.HiddenForReceiver = true;
            }
            else if (hideForUser)
            {
                message.HiddenForSender = true;
            }

            if (hideForUser || hideForReceiverUser)
            {
                _repository.Edit(message);
                await _unitOfWork.CommitAsync();
                return ServiceResult.Success();
            }
            return ServiceResult.Failure("Nie ma możliwości usunięcia tej wiadomości.");
        }

        public async Task<ServiceResult> DeleteSingleByAdmin(int userId, Guid messageId)
        {
            var user = await _userRepository.FindAsync(userId);
            if (!user.IsAdmin)
            {
                return ServiceResult.Failure("Wystąpił błąd autoryzacji superużytkownika.");
            }
            var nextMessage = await _repository.SingleOrDefaultAsync(x => x.PreviousMessageId == messageId);
            var message = await _repository.Include(x => x.PreviousMessage).SingleOrDefaultAsync(x => x.Id == messageId);
            var previousMessage = message.PreviousMessage;

            if (nextMessage != null)
            {
                nextMessage.PreviousMessageId = previousMessage.Id;
                _repository.Edit(nextMessage);
            }
            _repository.Delete(message);
            await _unitOfWork.CommitAsync();
            return ServiceResult.Success();
        }

        //TODO
        public Task<ServiceResult> DeleteClusterByAdmin(int userId, Guid messageId)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<PortalMessageDto>> ValidateMessageRecipents(int userId, Guid previousMessageId)
        {
            var message = await _repository.FindAsync(previousMessageId);
            if (message == null)
            {
                return ServiceResult<PortalMessageDto>.Failure("Wystąpił błąd podczas tworzenia wiadomości");
            }
            if (message.UserId != userId && message.ReceiverUserId != userId)
            {
                return ServiceResult<PortalMessageDto>.Failure("Nie oszukuj!");
            }
            return ServiceResult<PortalMessageDto>.Success(_mapper.Map<PortalMessageDto>(message));
        }

        private static void PushToCurrentMessageStack(Stack<PortalMessage> currentStack, List<PortalMessage> allUserMessages)
        {
            while (true)
            {
                var currentMessage = currentStack.Peek();
                var nextMessage = allUserMessages.SingleOrDefault(x => x.PreviousMessageId == currentMessage.Id);
                if (nextMessage != null)
                {
                    currentStack.Push(nextMessage);
                    continue;
                }
                break;
            }
        }

        private PortalMessageUserDto CreateEmptyPlaceholderForDeletedUser()
        {
            return new PortalMessageUserDto
            {
                Email = "Konto usunięte",
                Id = 0,
                ImgId = "avatar-placeholder",
                Initials = "Konto usunięte"
            };
        }
    }
}
