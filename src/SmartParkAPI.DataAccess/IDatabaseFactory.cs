using System;
using SmartParkAPI.Model;

namespace SmartParkAPI.DataAccess
{
    public interface IDatabaseFactory:IDisposable
    {
        ParkingAthContext Get();
    }
}
