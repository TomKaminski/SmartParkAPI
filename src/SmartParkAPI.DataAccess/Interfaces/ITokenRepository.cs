using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.Model.Concrete;

namespace SmartParkAPI.DataAccess.Interfaces
{
    public interface ITokenRepository:IGenericRepository<Token, long>, IDependencyRepository
    {
        
    }
}
