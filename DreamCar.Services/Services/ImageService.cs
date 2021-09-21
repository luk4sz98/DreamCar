using AutoMapper;
using DreamCar.DAL.EF;
using DreamCar.Model.DataModels;
using DreamCar.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DreamCar.Services.Services
{
    public class ImageService : BaseService, IImageService
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        public ImageService(
            ApplicationDbContext dbContext,
            IMapper mapper,
            ILogger logger,
            UserManager<User> userManager,
            IWebHostEnvironment hostEnvironment)
            : base(dbContext, mapper, logger, userManager)
        {
            _hostEnvironment = hostEnvironment;
        }

        public async Task SaveImages(IFormFileCollection images, Guid advertId)
        {
            try
            {
                Directory.CreateDirectory(
                    Path.GetFullPath(
                        Path.Combine(_hostEnvironment.WebRootPath, @"advertImages\\Advert " + advertId)
                        )
                    );

                foreach (var image in images)
                {
                    var filename = Guid.NewGuid() + Path.GetExtension(image.FileName);
                    var path = Path.GetFullPath(
                                    Path.Combine(
                                            _hostEnvironment.WebRootPath, @"advertImages\\Advert " + advertId
                                        ));
                    using var filestream = new FileStream(Path.Combine(path, filename), FileMode.Create);
                    await image.CopyToAsync(filestream);
                    await DbContext.Images.AddAsync(
                        new Image { 
                            AdvertId = advertId,
                            FileName = filename
                        });
                }

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
            }
        }
    }
}
