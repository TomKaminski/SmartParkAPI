using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.Model.Concrete;

namespace SmartParkAPI.DataAccess.Interfaces
{
    public interface IPriceTresholdRepository:IGenericRepository<PriceTreshold, int>, IDependencyRepository
    {
    }
}
