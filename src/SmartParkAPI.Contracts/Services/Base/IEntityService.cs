using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SmartParkAPI.Contracts.Common;

namespace SmartParkAPI.Contracts.Services.Base
{
    public interface IEntityService<TDto, T>
        where TDto : BaseDto<T>
        where T : struct
    {
        //sync methods
        ServiceResult<int> Count();
        ServiceResult<int> Count(Expression<Func<TDto, bool>> predicate);

        ServiceResult<TDto> Get(T id);
        ServiceResult<TDto> Get(Expression<Func<TDto, bool>> predicate);

        ServiceResult<TDto> Create(TDto entity);
        ServiceResult CreateMany(IEnumerable<TDto> entities);

        ServiceResult Edit(TDto entity);
        ServiceResult EditMany(IList<TDto> entities);

        ServiceResult Delete(TDto entity);
        ServiceResult DeleteMany(IEnumerable<TDto> entities);
        ServiceResult Delete(T id);
        ServiceResult DeleteMany(IEnumerable<T> ids);

        ServiceResult<IEnumerable<TDto>> GetAll();
        ServiceResult<IEnumerable<TDto>> GetAll(Expression<Func<TDto, bool>> predicate);


        //async methods
        Task<ServiceResult<TDto>> GetAsync(T id);
        Task<ServiceResult<TDto>> GetAsync(Expression<Func<TDto, bool>> predicate);

        Task<ServiceResult<TDto>> CreateAsync(TDto entity);
        Task<ServiceResult> CreateManyAsync(IEnumerable<TDto> entities);

        Task<ServiceResult<TDto>> EditAsync(TDto entity);

        Task<ServiceResult> EditManyAsync(IList<TDto> entities);

        Task<ServiceResult> DeleteAsync(TDto entity);
        Task<ServiceResult> DeleteManyAsync(IEnumerable<TDto> entities);
        Task<ServiceResult> DeleteAsync(T id);
        Task<ServiceResult> DeleteManyAsync(IEnumerable<T> ids);

        Task<ServiceResult<IEnumerable<TDto>>> GetAllAsync();
        Task<ServiceResult<IEnumerable<TDto>>> GetAllAsync(Expression<Func<TDto, bool>> predicate);
    }
}
