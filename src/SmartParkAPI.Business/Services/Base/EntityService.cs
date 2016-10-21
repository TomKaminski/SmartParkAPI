using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Contracts.Services.Base;
using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.Model.Common;
using SmartParkAPI.Shared.Helpers;

namespace SmartParkAPI.Business.Services.Base
{
    public abstract class EntityService<TDto, TEntity,T> : IEntityService<TDto,T>
      where TDto : BaseDto<T>
      where TEntity : Entity<T>
      where T: struct 
    {
        private readonly IGenericRepository<TEntity,T> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        protected EntityService(IGenericRepository<TEntity,T> repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public ServiceResult<int> Count()
        {
            return ServiceResult<int>.Success(_repository.Count());
        }

        public ServiceResult<int> Count(Expression<Func<TDto, bool>> predicate)
        {
            return ServiceResult<int>.Success(_repository.Count(MapExpressionToEntity(predicate)));
        }

        public virtual ServiceResult<TDto> Get(T id)
        {
            var result = _mapper.Map<TDto>(_repository.Find(id));
            return ServiceResult<TDto>.Success(result);
        }

        public virtual ServiceResult<TDto> Get(Expression<Func<TDto, bool>> predicate)
        {
            var result = _mapper.Map<TDto>(_repository.FirstOrDefault(MapExpressionToEntity(predicate)));
            return ServiceResult<TDto>.Success(result);
        }



        public virtual ServiceResult<TDto> Create(TDto entity)
        {
            var result = _mapper.Map<TDto>(_repository.Add(_mapper.Map<TEntity>(entity)));
            _unitOfWork.Commit();
            return ServiceResult<TDto>.Success(result);
        }

        public virtual ServiceResult CreateMany(IEnumerable<TDto> entities)
        {
            foreach (var entity in entities)
            {
                _repository.Add(_mapper.Map<TEntity>(entity));
            }
            _unitOfWork.Commit();
            return ServiceResult.Success();
        }

        public virtual ServiceResult Edit(TDto entity)
        {
            var obj = _repository.Find(entity.Id);
            if (obj == null)
            {
                return ServiceResult.Failure(new List<string> { "Nie znaleziono pasującego elementu." });
            }
            obj = _mapper.Map<TEntity>(entity);
            //obj = MapperHelper<TDto, TEntity>.MapNoIdToEntityOnEdit(entity, obj);
            _repository.Edit(obj);
            _unitOfWork.Commit();
            return ServiceResult.Success();
        }

        public virtual ServiceResult EditMany(IList<TDto> entities)
        {
            var ids = entities.Select(k => k.Id).ToList();
            var dbEntities = _repository.GetAll(x => ids.Contains(x.Id)).ToList();

            foreach (var t in entities)
            {
                var entity = dbEntities.Single(x => x.Id.Equals(t.Id));
                entity = MapperHelper<TDto, TEntity>.MapNoIdToEntityOnEdit(t, entity);
                _repository.Edit(entity);
            }
            _unitOfWork.Commit();
            return ServiceResult.Success();
        }

        public virtual ServiceResult Delete(TDto entity)
        {
            _repository.Delete(_mapper.Map<TEntity>(entity));
            _unitOfWork.Commit();
            return ServiceResult.Success();
        }

        public virtual ServiceResult DeleteMany(IEnumerable<TDto> entities)
        {
            foreach (var entity in entities)
            {
                _repository.Delete(_mapper.Map<TEntity>(entity));
            }
            _unitOfWork.Commit();
            return ServiceResult.Success();
        }

        public virtual ServiceResult Delete(T id)
        {
            var obj = _repository.Find(id);
            _repository.Delete(obj);
            _unitOfWork.Commit();
            return ServiceResult.Success();
        }

        public virtual ServiceResult DeleteMany(IEnumerable<T> ids)
        {
            var dbEntities = _repository.GetAll(x => ids.Contains(x.Id)).ToList();

            foreach (var t in dbEntities)
            {
                _repository.Delete(t);
            }
            _unitOfWork.Commit();
            return ServiceResult.Success();
        }

        public virtual ServiceResult<IEnumerable<TDto>> GetAll()
        {
            return ServiceResult<IEnumerable<TDto>>
                .Success(_repository.GetAll()
                .Select(_mapper.Map<TDto>).ToList());
        }

        public virtual ServiceResult<IEnumerable<TDto>> GetAll(Expression<Func<TDto, bool>> predicate)
        {
            return ServiceResult<IEnumerable<TDto>>
                .Success(_repository.GetAll(MapExpressionToEntity(predicate))
                .Select(_mapper.Map<TDto>).ToList());
        }

        public virtual async Task<ServiceResult<TDto>> GetAsync(T id)
        {
            return ServiceResult<TDto>.Success(_mapper.Map<TDto>(await _repository.FindAsync(id)));
        }

        public virtual async Task<ServiceResult<TDto>> GetAsync(Expression<Func<TDto, bool>> predicate)
        {
            return ServiceResult<TDto>
                .Success(_mapper.Map<TDto>(await _repository.FirstOrDefaultAsync(MapExpressionToEntity(predicate))));
        }

        public virtual async Task<ServiceResult<TDto>> CreateAsync(TDto entity)
        {
            var item = _repository.Add(_mapper.Map<TEntity>(entity));
            await _unitOfWork.CommitAsync();
            return ServiceResult<TDto>.Success(_mapper.Map<TDto>(item));
        }

        public virtual async Task<ServiceResult> CreateManyAsync(IEnumerable<TDto> entities)
        {
            foreach (var entity in entities)
            {
                _repository.Add(_mapper.Map<TEntity>(entity));
            }
            await _unitOfWork.CommitAsync();
            return ServiceResult.Success();
        }

        public virtual async Task<ServiceResult<TDto>> EditAsync(TDto entity)
        {
            var obj = await _repository.FindAsync(entity.Id);
            if (obj == null)
            {
                return ServiceResult<TDto>.Failure(new List<string> { "Nie znaleziono pasującego elementu." });
            }

            obj = MapperHelper<TDto, TEntity>.MapNoIdToEntityOnEdit(entity, obj);
            _repository.Edit(obj);
            await _unitOfWork.CommitAsync();
            return ServiceResult<TDto>.Success(_mapper.Map<TDto>(obj));
        }

        public virtual async Task<ServiceResult> EditManyAsync(IList<TDto> entities)
        {
            var ids = entities.Select(k => k.Id).ToList();
            var dbEntities = _repository.GetAll(x => ids.Contains(x.Id)).ToList();

            foreach (var t in entities)
            {
                var entity = dbEntities.Single(x => x.Id.Equals(t.Id));
                entity = MapperHelper<TDto, TEntity>.MapNoIdToEntityOnEdit(t, entity);
                _repository.Edit(entity);
            }
            await _unitOfWork.CommitAsync();
            return ServiceResult.Success();
        }

        public virtual async Task<ServiceResult> DeleteAsync(TDto entity)
        {
            _repository.Delete(_mapper.Map<TEntity>(entity));
            await _unitOfWork.CommitAsync();
            return ServiceResult.Success();
        }

        public virtual async Task<ServiceResult> DeleteManyAsync(IEnumerable<TDto> entities)
        {
            foreach (var entity in entities)
            {
                _repository.Delete(_mapper.Map<TEntity>(entity));
            }
            await _unitOfWork.CommitAsync();
            return ServiceResult.Success();
        }

        public virtual async Task<ServiceResult> DeleteAsync(T id)
        {
            var obj = await _repository.FindAsync(id);
            _repository.Delete(obj);
            await _unitOfWork.CommitAsync();
            return ServiceResult.Success();
        }

        public virtual async Task<ServiceResult> DeleteManyAsync(IEnumerable<T> ids)
        {
            var dbEntities = (await _repository.GetAllAsync(x => ids.Contains(x.Id))).ToList();

            foreach (var t in dbEntities)
            {
                _repository.Delete(t);
            }
            await _unitOfWork.CommitAsync();
            return ServiceResult.Success();
        }

        public virtual async Task<ServiceResult<IEnumerable<TDto>>> GetAllAsync()
        {
            return ServiceResult<IEnumerable<TDto>>
                .Success((await _repository.GetAllAsync())
                .Select(_mapper.Map<TDto>).ToList());
        }

        public virtual async Task<ServiceResult<IEnumerable<TDto>>> GetAllAsync(Expression<Func<TDto, bool>> predicate)
        {
            return ServiceResult<IEnumerable<TDto>>
                .Success((await _repository.GetAllAsync(MapExpressionToEntity(predicate)))
                .Select(_mapper.Map<TDto>).ToList());
        }

        protected static Expression<Func<TEntity, bool>> MapExpressionToEntity(Expression<Func<TDto, bool>> predicate)
        {
            var param = Expression.Parameter(typeof(TEntity));
            var result = new CustomExpressionVisitor<TEntity>(param).Visit(predicate.Body);
            return Expression.Lambda<Func<TEntity, bool>>(result, param);
        }
    }
}
