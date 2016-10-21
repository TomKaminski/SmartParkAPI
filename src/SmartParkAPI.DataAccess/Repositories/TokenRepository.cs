using Microsoft.EntityFrameworkCore;
using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.DataAccess.Interfaces;
using SmartParkAPI.Model.Concrete;

namespace SmartParkAPI.DataAccess.Repositories
{
    public class TokenRepository : GenericRepository<Token, long>, ITokenRepository
    {
        private readonly DbSet<Token> _dbset;

        public TokenRepository(IDatabaseFactory factory)
            : base(factory)
        {
            _dbset = factory.Get().Set<Token>();
        }

    }
}
