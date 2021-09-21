using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace DreamCar.Services.Interfaces
{
    public interface IImageService
    {
        Task SaveImages(IFormFileCollection images, Guid advertId);
    }
}
