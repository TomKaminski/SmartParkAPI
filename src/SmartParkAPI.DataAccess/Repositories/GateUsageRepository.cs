using System;
using Microsoft.EntityFrameworkCore;
using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.DataAccess.Interfaces;
using SmartParkAPI.Model.Concrete;

namespace SmartParkAPI.DataAccess.Repositories
{
    public class GateUsageRepository : GenericRepository<GateUsage, Guid>, IGateUsageRepository
    {
        private readonly DbSet<GateUsage> _dbset;

        public GateUsageRepository(IDatabaseFactory factory)
            : base(factory)
        {
            _dbset = factory.Get().Set<GateUsage>();
        }
    }
}
