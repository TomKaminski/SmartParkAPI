using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SmartParkAPI.DataAccess.Common
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private DbContext _dbContext;

        public UnitOfWork(IDatabaseFactory factory)
        {
            _dbContext = factory.Get();
        }

        public int Commit()
        {
            try
            {
                return _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                //TODO: logger
                //_logger.log(e)
                throw;
            }
        }

        public async Task<int> CommitAsync()
        {
            try
            {
                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                //TODO: logger
                //_logger.log(e)
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (_dbContext == null) return;
            _dbContext.Dispose();
            _dbContext = null;
        }
    }
}
