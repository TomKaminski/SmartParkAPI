using System;
using System.Threading.Tasks;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Contracts.DTO.PortalMessage;
using SmartParkAPI.Contracts.Services.Base;

namespace SmartParkAPI.Contracts.Services
{
    public interface IPortalMessageService : IEntityService<PortalMessageDto, Guid>, IDependencyService
    {
        Task<ServiceResult<PortalMessageClustersDto>> GetPortalMessageClusterForCurrentUserAsync(int userId);

        Task<ServiceResult> FakeDelete(Guid messageId, int userId);

        Task<ServiceResult> DeleteSingleByAdmin(int userId, Guid messageId);
        Task<ServiceResult> DeleteClusterByAdmin(int userId, Guid messageId);
        Task<ServiceResult<PortalMessageDto>> ValidateMessageRecipents(int userId, Guid previousMessageId);
        Task<ServiceResult> FakeDeleteCluster(int userId, Guid starterMessageId);
        Task<ServiceResult<int>> GetUnreadClustersCountAsync(int userId);
    }
}
