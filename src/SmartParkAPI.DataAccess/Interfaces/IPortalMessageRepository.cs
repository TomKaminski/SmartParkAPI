using System;
using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.Model.Concrete;

namespace SmartParkAPI.DataAccess.Interfaces
{
    public interface IPortalMessageRepository:IGenericRepository<PortalMessage,Guid>, IDependencyRepository
    {
    }
}
