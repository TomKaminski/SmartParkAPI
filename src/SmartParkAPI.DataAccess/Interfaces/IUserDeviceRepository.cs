using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.Model.Concrete;

namespace SmartParkAPI.DataAccess.Interfaces
{
    public interface IUserDeviceRepository : IGenericRepository<UserDevice, int>, IDependencyRepository
    {
    }
}
