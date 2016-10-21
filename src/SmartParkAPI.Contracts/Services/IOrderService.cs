using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Contracts.DTO.Order;
using SmartParkAPI.Contracts.Services.Base;
using SmartParkAPI.Shared.Enums;

namespace SmartParkAPI.Contracts.Services
{
    public interface IOrderService : IEntityService<OrderBaseDto,long>, IDependencyService
    {
        Task<ServiceResult<Guid>> GenerateExternalOrderIdAsync();
        Task<ServiceResult<OrderBaseDto>> GetAsync(Guid externalorderId);

        ServiceResult<IEnumerable<OrderAdminDto>> GetAllAdmin();
        ServiceResult<IEnumerable<OrderAdminDto>> GetAllAdmin(Expression<Func<OrderAdminDto, bool>> predicate);

        Task<ServiceResult<IEnumerable<OrderAdminDto>>> GetAllAdminAsync();
        Task<ServiceResult<IEnumerable<OrderAdminDto>>> GetAllAdminAsync(Expression<Func<OrderAdminDto, bool>> predicate);

        Task<ServiceResult<OrderStatus>> UpdateOrderState(string status, Guid extOrderId);
    }
}
