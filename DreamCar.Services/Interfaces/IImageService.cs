using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace DreamCar.Services.Interfaces
{
    public interface IImageService
    {
        Task SaveUploadedImagesAsync(IFormFileCollection images, Guid advertId);
        void DeleteSavedImages(Guid advertId);
    }
}
