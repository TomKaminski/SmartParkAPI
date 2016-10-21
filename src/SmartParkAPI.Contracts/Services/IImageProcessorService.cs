using System;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Contracts.Services.Base;

namespace SmartParkAPI.Contracts.Services
{
    public interface IImageProcessorService : IDependencyService
    {
        ServiceResult<byte[], Guid> ProcessAndSaveImage(byte[] source, string path);
        void DeleteImagesByPath(string path);
    }
}
