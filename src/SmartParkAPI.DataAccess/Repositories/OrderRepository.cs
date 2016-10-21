using Microsoft.EntityFrameworkCore;
using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.DataAccess.Interfaces;
using SmartParkAPI.Model.Concrete;

namespace SmartParkAPI.DataAccess.Repositories
{
    public class OrderRepository:GenericRepository<Order, long>,IOrderRepository
    {
        private readonly DbSet<Order> _dbset;

        public OrderRepository(IDatabaseFactory factory)
            : base(factory)
        {
            _dbset = factory.Get().Set<Order>();
        }
    }
}
