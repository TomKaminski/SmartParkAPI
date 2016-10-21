using System;
using System.IO;
using ImageResizer;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Contracts.Services;

namespace SmartParkAPI.Business.Services
{
    public class ImageProcessorService : IImageProcessorService
    {
        public ServiceResult<byte[], Guid> ProcessAndSaveImage(byte[] source, string path)
        {
            var guid = Guid.NewGuid();
            var imageJob = new ImageJob(source, path + guid, new Instructions("width=300&height=300&format=jpg"))
            {
                CreateParentDirectory = true,
                AddFileExtension = true,
            };
            imageJob.Build();
            return ServiceResult<byte[], Guid>.Success(File.ReadAllBytes(imageJob.FinalPath), guid);
        }

        public void DeleteImagesByPath(string path)
        {
            if (File.Exists($"{path}.jpg"))
                File.Delete($"{path}.jpg");
        }
    }
}
