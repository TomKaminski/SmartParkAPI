using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Contracts.DTO.PriceTreshold;
using SmartParkAPI.Contracts.Services.Base;

namespace SmartParkAPI.Contracts.Services
{
    public interface IPriceTresholdService:IEntityService<PriceTresholdBaseDto,int>, IDependencyService
    {
        ServiceResult<IEnumerable<PriceTresholdAdminDto>> GetAllAdmin();
        ServiceResult<IEnumerable<PriceTresholdAdminDto>> GetAllAdmin(Expression<Func<PriceTresholdAdminDto, bool>> predicate);

        Task<ServiceResult<IEnumerable<PriceTresholdAdminDto>>> GetAllAdminAsync();
        Task<ServiceResult<IEnumerable<PriceTresholdAdminDto>>> GetAllAdminAsync(Expression<Func<PriceTresholdAdminDto, bool>> predicate);

        Task<ServiceResult> RecoverPriceTresholdAsync(int id);

        new Task<ServiceResult<PriceTresholdBaseDto, PrcAdminCreateInfo>> CreateAsync(PriceTresholdBaseDto entity);
        new ServiceResult<PriceTresholdBaseDto, PrcAdminCreateInfo> Create(PriceTresholdBaseDto entity);

    }
}
