using System;
using Microsoft.EntityFrameworkCore;
using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.DataAccess.Interfaces;
using SmartParkAPI.Model.Concrete;

namespace SmartParkAPI.DataAccess.Repositories
{
    public class PortalMessageRepository : GenericRepository<PortalMessage, Guid>, IPortalMessageRepository
    {
        private readonly DbSet<PortalMessage> _dbset;

        public PortalMessageRepository(IDatabaseFactory factory)
            : base(factory)
        {
            
            _dbset = factory.Get().Set<PortalMessage>();
        }
    }
}
