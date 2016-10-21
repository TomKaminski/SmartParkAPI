using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartParkAPI.Business.Services.Base;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Contracts.DTO.PriceTreshold;
using SmartParkAPI.Contracts.Services;
using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.DataAccess.Interfaces;
using SmartParkAPI.Model.Concrete;
using SmartParkAPI.Shared.Helpers;

namespace SmartParkAPI.Business.Services
{
    public class PriceTresholdService : EntityService<PriceTresholdBaseDto, PriceTreshold, int>, IPriceTresholdService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPriceTresholdRepository _repository;
        private readonly IMapper _mapper;

        public PriceTresholdService(IUnitOfWork unitOfWork, IPriceTresholdRepository repository, IMapper mapper) : base(repository, unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _mapper = mapper;
        }

        public override async Task<ServiceResult<IEnumerable<PriceTresholdBaseDto>>> GetAllAsync()
        {
            var count = Count();
            if (count.Result == 0)
            {
                _repository.Add(new PriceTreshold
                {
                    MinCharges = 0,
                    PricePerCharge = 3
                });
                await _unitOfWork.CommitAsync();
            }
            return ServiceResult<IEnumerable<PriceTresholdBaseDto>>.Success(_repository.GetAll(x => !x.IsDeleted).OrderBy(x => x.MinCharges).Select(_mapper.Map<PriceTresholdBaseDto>));
        }

        public new async Task<ServiceResult<PriceTresholdBaseDto, PrcAdminCreateInfo>> CreateAsync(PriceTresholdBaseDto entity)
        {
            var createInfo = new PrcAdminCreateInfo();

            var recoverItem = await _repository.FirstOrDefaultAsync(x => x.IsDeleted && x.MinCharges == entity.MinCharges && x.PricePerCharge == entity.PricePerCharge);

            if (recoverItem != null)
            {
                recoverItem.IsDeleted = false;
                _repository.Edit(recoverItem);
                await _unitOfWork.CommitAsync();
                createInfo.Recovered = true;
                return ServiceResult<PriceTresholdBaseDto, PrcAdminCreateInfo>.Success(_mapper.Map<PriceTresholdBaseDto>(recoverItem), createInfo);
            }

            var conflictingItems = await _repository.GetAllAsync(x => !x.IsDeleted && (x.MinCharges == entity.MinCharges || x.PricePerCharge == entity.PricePerCharge));

            if (conflictingItems.Any())
            {
                if (entity.MinCharges == 0)
                {
                    var currentDefault = await _repository.FirstOrDefaultAsync(x => !x.IsDeleted && x.MinCharges == 0);
                    currentDefault.IsDeleted = true;

                    foreach (var conflictingItem in conflictingItems)
                    {
                        conflictingItem.IsDeleted = true;
                        _repository.Edit(conflictingItem);
                    }

                    createInfo.ReplacedDefault = true;
                }
                else
                {
                    return ServiceResult<PriceTresholdBaseDto, PrcAdminCreateInfo>
                        .Failure($"Podane wartości kolidują z już istniejącym przedziałem (min. wyjazdy: {conflictingItems.First().MinCharges}, cena za szt.: {conflictingItems.First().PricePerCharge.ToString("##.00")})");
                }
            }

            var newPrc = _repository.Add(_mapper.Map<PriceTreshold>(entity));
            await _unitOfWork.CommitAsync();
            return ServiceResult<PriceTresholdBaseDto, PrcAdminCreateInfo>.Success(_mapper.Map<PriceTresholdBaseDto>(newPrc), createInfo);
        }

        public new ServiceResult<PriceTresholdBaseDto, PrcAdminCreateInfo> Create(PriceTresholdBaseDto entity)
        {
            var createInfo = new PrcAdminCreateInfo();

            var recoverItem = _repository.FirstOrDefault(x => x.IsDeleted && x.MinCharges == entity.MinCharges && x.PricePerCharge == entity.PricePerCharge);

            if (recoverItem != null)
            {
                recoverItem.IsDeleted = false;
                _repository.Edit(recoverItem);
                _unitOfWork.Commit();
                createInfo.Recovered = true;
                return ServiceResult<PriceTresholdBaseDto, PrcAdminCreateInfo>.Success(_mapper.Map<PriceTresholdBaseDto>(recoverItem), createInfo);
            }

            var conflictingItems = _repository.GetAll(x => !x.IsDeleted && (x.MinCharges == entity.MinCharges || x.PricePerCharge == entity.PricePerCharge));

            if (conflictingItems.Any())
            {
                if (entity.MinCharges == 0)
                {
                    var currentDefault = _repository.FirstOrDefault(x => !x.IsDeleted && x.MinCharges == 0);
                    currentDefault.IsDeleted = true;

                    foreach (var conflictingItem in conflictingItems)
                    {
                        conflictingItem.IsDeleted = true;
                        _repository.Edit(conflictingItem);
                    }

                    createInfo.ReplacedDefault = true;
                }
                else
                {
                    return ServiceResult<PriceTresholdBaseDto, PrcAdminCreateInfo>
                        .Failure($"Podane wartości kolidują z już istniejącym przedziałem (min. wyjazdy: {conflictingItems.First().MinCharges}, cena za szt.: {conflictingItems.First().PricePerCharge.ToString("##.00")})");
                }
            }

            var newPrc = _repository.Add(_mapper.Map<PriceTreshold>(entity));
            _unitOfWork.Commit();
            return ServiceResult<PriceTresholdBaseDto, PrcAdminCreateInfo>.Success(_mapper.Map<PriceTresholdBaseDto>(newPrc), createInfo);
        }

        public ServiceResult<IEnumerable<PriceTresholdAdminDto>> GetAllAdmin()
        {
            return ServiceResult<IEnumerable<PriceTresholdAdminDto>>
                .Success(_repository.Include(x => x.Orders)
                .Select(_mapper.Map<PriceTresholdAdminDto>));
        }

        public ServiceResult<IEnumerable<PriceTresholdAdminDto>> GetAllAdmin(Expression<Func<PriceTresholdAdminDto, bool>> predicate)
        {
            var param = Expression.Parameter(typeof(PriceTreshold));
            var result = new CustomExpressionVisitor<PriceTreshold>(param).Visit(predicate.Body);
            var lambda = Expression.Lambda<Func<PriceTreshold, bool>>(result, param);
            return ServiceResult<IEnumerable<PriceTresholdAdminDto>>
                .Success(_repository.Include(x => x.Orders).Where(lambda)
                .Select(_mapper.Map<PriceTresholdAdminDto>));
        }

        public async Task<ServiceResult<IEnumerable<PriceTresholdAdminDto>>> GetAllAdminAsync()
        {
            return ServiceResult<IEnumerable<PriceTresholdAdminDto>>
                .Success(
                    (await _repository.Include(x => x.Orders).ToListAsync()).Select(_mapper.Map<PriceTresholdAdminDto>));
        }

        public async Task<ServiceResult<IEnumerable<PriceTresholdAdminDto>>> GetAllAdminAsync(Expression<Func<PriceTresholdAdminDto, bool>> predicate)
        {
            var param = Expression.Parameter(typeof(PriceTreshold));
            var result = new CustomExpressionVisitor<PriceTreshold>(param).Visit(predicate.Body);
            var lambda = Expression.Lambda<Func<PriceTreshold, bool>>(result, param);

            var count = Count();
            if (count.Result == 0)
            {
                _repository.Add(new PriceTreshold
                {
                    MinCharges = 0,
                    PricePerCharge = 3
                });
                await _unitOfWork.CommitAsync();
            }

            return ServiceResult<IEnumerable<PriceTresholdAdminDto>>
                .Success((await _repository.Include(x => x.Orders).Where(lambda).ToListAsync()).Select(_mapper.Map<PriceTresholdAdminDto>));
        }

        public async Task<ServiceResult> RecoverPriceTresholdAsync(int id)
        {
            var obj = await _repository.FindAsync(id);

            var conflictingItem = _repository.FirstOrDefault(x => x.IsDeleted != true && (x.MinCharges == obj.MinCharges || x.PricePerCharge == obj.PricePerCharge));

            if (conflictingItem != null)
            {
                if (conflictingItem.MinCharges == 0 && obj.MinCharges == 0)
                {
                    conflictingItem.IsDeleted = true;
                    _repository.Edit(conflictingItem);

                    obj.IsDeleted = false;
                    _repository.Edit(obj);
                    await _unitOfWork.CommitAsync();
                    return ServiceResult.Success("Przywrócono przedział cenowy, oraz podmieniono już z isntiejącym - kolidującym");
                }
                return ServiceResult<PriceTresholdBaseDto, PrcAdminCreateInfo>
                        .Failure($"Nie można przywrócić przedziału, koliduje z przedziałem o wartościach: (min. wyjazdy: {conflictingItem.MinCharges}, cena za szt.: {conflictingItem.PricePerCharge.ToString("##.00")})");
            }
            obj.IsDeleted = false;
            _repository.Edit(obj);
            await _unitOfWork.CommitAsync();
            return ServiceResult.Success("Przywrócono przedział cenowy.");
        }

        public override async Task<ServiceResult> DeleteAsync(int id)
        {
            var obj = await _repository.FindAsync(id);
            if (obj.MinCharges == 0)
            {
                return ServiceResult.Failure("Nie można usunąć bazowego przedziału!");
            }
            obj.IsDeleted = true;
            _repository.Edit(obj);
            await _unitOfWork.CommitAsync();
            return ServiceResult.Success();
        }

        public override ServiceResult Delete(int id)
        {
            var obj = _repository.Find(id);
            if (obj.MinCharges == 0)
            {
                return ServiceResult.Failure("Nie można usunąć bazowego przedziału!");
            }
            obj.IsDeleted = true;
            _repository.Edit(obj);
            _unitOfWork.Commit();
            return ServiceResult.Success();
        }
    }
}
