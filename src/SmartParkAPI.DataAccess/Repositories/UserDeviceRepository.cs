using Microsoft.EntityFrameworkCore;
using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.DataAccess.Interfaces;
using SmartParkAPI.Model.Concrete;

namespace SmartParkAPI.DataAccess.Repositories
{
    public class UserDeviceRepository : GenericRepository<UserDevice, int>, IUserDeviceRepository
    {
        private readonly DbSet<UserDevice> _dbset;

        public UserDeviceRepository(IDatabaseFactory factory)
            : base(factory)
        {
            _dbset = factory.Get().Set<UserDevice>();
        }
    }
}
