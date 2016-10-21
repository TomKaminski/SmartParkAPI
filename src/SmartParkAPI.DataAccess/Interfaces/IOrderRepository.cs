using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.Model.Concrete;

namespace SmartParkAPI.DataAccess.Interfaces
{
    public interface IOrderRepository:IGenericRepository<Order, long>, IDependencyRepository
    {
    }
}
