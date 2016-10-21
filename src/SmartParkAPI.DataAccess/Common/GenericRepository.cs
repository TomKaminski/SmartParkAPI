using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartParkAPI.Model.Common;

namespace SmartParkAPI.DataAccess.Common
{
    public abstract class GenericRepository<T,TType> : IGenericRepository<T, TType> 
        where T : Entity<TType>
        where TType : struct 
    {
        private readonly DbContext _entities;
        private readonly DbSet<T> _dbset;

        protected GenericRepository(IDatabaseFactory factory)
        {
            _entities = factory.Get();
            _dbset = _entities.Set<T>();
        }

        public T Add(T entity)
        {
            _dbset.Add(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            _dbset.Remove(entity);
        }

        public void Edit(T entity)
        {
            _entities.Entry(entity).State = EntityState.Modified;
        }

        public int Count()
        {
            return _dbset.Count();
        }

        public int Count(Expression<Func<T, bool>> expression)
        {
            return _dbset.Count(expression);
        }

        public IQueryable<T> Include(Expression<Func<T, object>> include)
        {
            return _dbset.Include(include);
        }


        //Sync
        public T Find(TType id)
        {
            return _dbset.AsNoTracking().First(x=>x.Id.Equals(id));
        }

        public T First(Expression<Func<T, bool>> expression)
        {
            return _dbset.AsNoTracking().First(expression);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> expression)
        {
            return _dbset.AsNoTracking().FirstOrDefault(expression);
        }

        public IQueryable<T> GetAll()
        {
            return _dbset.AsNoTracking();
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> expression)
        {
            return _dbset.AsNoTracking().Where(expression);
        }

        //Async
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbset.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbset.AsNoTracking().Where(expression).ToListAsync();
        }

        public async Task<T> FindAsync(TType id)
        {
            return await _dbset.AsNoTracking().FirstAsync(x=>x.Id.Equals(id));
        }

        public async Task<T> FirstAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbset.AsNoTracking().FirstAsync(expression);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbset.AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbset.AsNoTracking().SingleOrDefaultAsync(expression);
        }
    }
}
