using System;
using System.Threading.Tasks;
using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.Model.Concrete;

namespace SmartParkAPI.DataAccess.Interfaces
{
    public interface IMessageRepository:IGenericRepository<Message,Guid>, IDependencyRepository
    {
        Task<Message> GetMessageByTokenId(long id);
    }
}
