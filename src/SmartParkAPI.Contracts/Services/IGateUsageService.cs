using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Contracts.DTO.GateUsage;
using SmartParkAPI.Contracts.Services.Base;

namespace SmartParkAPI.Contracts.Services
{
    public interface IGateUsageService : IEntityService<GateUsageBaseDto, Guid>, IDependencyService
    {
        Task<ServiceResult<IEnumerable<GateUsageAdminDto>>> GetAllAdminAsync();

        Task<ServiceResult<IEnumerable<GateUsageAdminDto>>> GetAllAdminAsync(Expression<Func<GateUsageBaseDto, bool>> predicate);
    }
}
