using System;
using System.Threading.Tasks;

namespace SmartParkAPI.DataAccess.Common
{
    public interface IUnitOfWork : IDisposable
    {
        int Commit();
        Task<int> CommitAsync();
    }
}
