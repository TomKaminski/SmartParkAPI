using Microsoft.EntityFrameworkCore;
using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.DataAccess.Interfaces;
using SmartParkAPI.Model.Concrete;

namespace SmartParkAPI.DataAccess.Repositories
{
    public class PriceTresholdRepository:GenericRepository<PriceTreshold, int>,IPriceTresholdRepository
    {
        private readonly DbSet<PriceTreshold> _dbSet;

         public PriceTresholdRepository(IDatabaseFactory factory)
            : base(factory)
        {
            _dbSet = factory.Get().Set<PriceTreshold>();
        }
    }
}
