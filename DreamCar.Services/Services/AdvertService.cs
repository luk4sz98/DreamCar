using AutoMapper;
using DreamCar.DAL.EF;
using DreamCar.Model.DataModels;
using DreamCar.Services.Interfaces;
using DreamCar.ViewModels.VM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DreamCar.Services.Services
{
    public class AdvertService : BaseService, IAdvertService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IImageService _imageService;

        public AdvertService(
            ApplicationDbContext dbContext,
            IMapper mapper,
            ILogger logger,
            UserManager<User> userManager,
            IHttpContextAccessor httpContext,
            IImageService imageService)
            : base(dbContext, mapper, logger, userManager)
        {
            _httpContext = httpContext;
            _imageService = imageService;
        }

        public async Task<(bool, string)> AddNewAdvertAsync(AddAdvertVm advert)
        {
            try
            {
                var carEntity = Mapper.Map<Car>(advert.Car);
                await DbContext.Cars.AddAsync(carEntity);

                var carEqu = new List<CarEquipment>();
                foreach (var equId in advert.Equipments.Checked)
                {
                    carEqu.Add(new CarEquipment { Car = carEntity, EquipmentId = equId });
                }
                await DbContext.CarEquipments.AddRangeAsync(carEqu);

                var advertEntity = Mapper.Map<Advert>(advert.Advert);               
                advertEntity.CreatedAt = DateTime.Now;
                advertEntity.User = await UserManager.GetUserAsync(_httpContext.HttpContext.User);
                advertEntity.Car = carEntity;                  
                await DbContext.Adverts.AddAsync(advertEntity);

                await _imageService.SaveImages(advert.Images.Image, advertEntity.Id);

                await DbContext.SaveChangesAsync();

                return (true, string.Empty);
            }
            catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
                return (false, ex.Message);
            }
        }
    }
}
