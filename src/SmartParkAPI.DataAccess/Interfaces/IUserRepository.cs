using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.Model.Concrete;

namespace SmartParkAPI.DataAccess.Interfaces
{
    public interface IUserRepository : IGenericRepository<User, int>, IDependencyRepository
    {
    }
}
