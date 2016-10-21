using Microsoft.EntityFrameworkCore;
using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.DataAccess.Interfaces;
using SmartParkAPI.Model.Concrete;

namespace SmartParkAPI.DataAccess.Repositories
{
    public class UserPreferencesRepository  : GenericRepository<UserPreferences, int>, IUserPreferencesRepository
    {
        private readonly DbSet<UserPreferences> _dbSet;

        public UserPreferencesRepository(IDatabaseFactory factory)
            : base(factory)
        {
            _dbSet = factory.Get().Set<UserPreferences>();
        }
    }
}
