using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.DataAccess.Interfaces;
using SmartParkAPI.Model.Concrete;

namespace SmartParkAPI.DataAccess.Repositories
{
    public class MessageRepository : GenericRepository<Message, Guid>, IMessageRepository
    {
        private readonly DbSet<Message> _dbset;

        public MessageRepository(IDatabaseFactory factory)
            : base(factory)
        {
            _dbset = factory.Get().Set<Message>();
        }

        public async Task<Message> GetMessageByTokenId(long id)
        {
            return await _dbset.Include(x => x.ViewInBrowserToken).FirstAsync(x => x.ViewInBrowserToken.Id == id);
        }
    }
}
