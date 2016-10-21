using System;
using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.Model.Concrete;

namespace SmartParkAPI.DataAccess.Interfaces
{
    public interface IGateUsageRepository:IGenericRepository<GateUsage,Guid>, IDependencyRepository
    {
    }
}
