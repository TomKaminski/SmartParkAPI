using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.Model.Concrete;

namespace SmartParkAPI.DataAccess.Interfaces
{
    public interface IUserPreferencesRepository : IGenericRepository<UserPreferences, int>, IDependencyRepository
    {
    }
}
