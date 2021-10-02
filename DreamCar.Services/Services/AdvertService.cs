using AutoMapper;
using DreamCar.DAL.EF;
using DreamCar.Model.DataModels;
using DreamCar.Services.Interfaces;
using DreamCar.ViewModels.VM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DreamCar.Services.Services
{
    public class AdvertService : BaseService, IAdvertService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IImageService _imageService;
        private readonly ICarService _carService;

        public AdvertService(
            ApplicationDbContext dbContext,
            IMapper mapper,
            ILogger logger,
            UserManager<User> userManager,
            IHttpContextAccessor httpContext,
            IImageService imageService,
            ICarService carService)
            : base(dbContext, mapper, logger, userManager)
        {
            _httpContext = httpContext;
            _imageService = imageService;
            _carService = carService;
        }

        public async Task<(bool, string)> AddNewAdvertAsync(AddAdvertVm advert)
        {
            try
            {

                var advertEntity = Mapper.Map<Advert>(advert.Advert);               
                
                advertEntity.CreatedAt = DateTime.Now;
                advertEntity.User = await UserManager.GetUserAsync(_httpContext.HttpContext.User);
                advertEntity.Car = await _carService.AddNewCarAsync(advert.Car, advert.Equipments.Checked)
                                   ?? throw new InvalidOperationException("Wystąpił problem z dodaniem pojazdu");

                if (string.IsNullOrEmpty(advertEntity.Title))
                    advertEntity.Title = string.Concat(advertEntity.Car.Brand, " ", advertEntity.Car.Model);
                
                await DbContext.Adverts.AddAsync(advertEntity);

                await _imageService.SaveUploadedImagesAsync(advert.ImagesUploaded.Images, advertEntity.Id);

                await DbContext.SaveChangesAsync();

                return (true, string.Empty);
            }
            catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
                return (false, ex.Message);
            }
        }

        public async Task<IEnumerable<UserAdvertVm>> GetUserAdvertsAsync(Expression<Func<Advert, bool>> filterExpressions)
        {
            try
            {
                var userAdvertsEntities = await DbContext.Adverts
                                                            .Where(filterExpressions)
                                                            .OrderByDescending(ad => ad.CreatedAt)
                                                            .ToListAsync();
                return Mapper.Map<IEnumerable<UserAdvertVm>>(userAdvertsEntities);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public IEnumerable<UserAdvertVm> GetUserAdvertsAsQueryable(string filterValue)
        {
            try
            {
                Expression<Func<Advert, bool>> filterExpressions = null;
                filterExpressions = ad => ad.Title
                                                .ToLower()
                                                .Contains(filterValue);

                var adverts = DbContext.Adverts.AsQueryable();
                adverts = adverts.Where(filterExpressions);
                
                return Mapper.Map<IEnumerable<UserAdvertVm>>(adverts);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return null;
            }
        }
    }
}
