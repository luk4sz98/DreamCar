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
        private readonly IEquipmentService _equipmentService;

        public AdvertService(
            ApplicationDbContext dbContext,
            IMapper mapper,
            ILogger logger,
            UserManager<User> userManager,
            IHttpContextAccessor httpContext,
            IImageService imageService,
            ICarService carService,
            IEquipmentService equipmentService)
            : base(dbContext, mapper, logger, userManager)
        {
            _httpContext = httpContext;
            _imageService = imageService;
            _carService = carService;
            _equipmentService = equipmentService;
        }

        public async Task<(bool, string)> AddOrEditAdvertAsync(AddOrEditAdvertVm advertVm)
        {
            try
            {
                // If Id has value, it means, it is edit operation
                if (advertVm.Advert.AdvertId.HasValue)
                {
                    var advertToUpdate = await DbContext.Adverts.FirstOrDefaultAsync(ad => ad.Id == advertVm.Advert.AdvertId);
                    UpdateAdvert(advertToUpdate, Mapper.Map<Advert>(advertVm.Advert));
                    await _carService.UpdateCar(advertToUpdate.Car, Mapper.Map<Car>(advertVm.Car), advertVm.Equipments.Checked);

                    if (advertVm.ImagesUploaded.Images is not null)
                        await _imageService.SaveUploadedImagesAsync(advertVm.ImagesUploaded.Images, advertToUpdate.Id);
                }
                else //if we are here, it means, it is new advert
                {
                    var advertEntity = Mapper.Map<Advert>(advertVm.Advert);

                    advertEntity.CreatedAt = DateTime.Now;
                    if (_httpContext.HttpContext != null)
                        advertEntity.User = await UserManager.GetUserAsync(_httpContext.HttpContext.User);
                    advertEntity.Car = await _carService.AddNewCarAsync(advertVm.Car, advertVm.Equipments.Checked)
                                       ?? throw new InvalidOperationException("Wystąpił problem z dodaniem pojazdu");

                    if (string.IsNullOrEmpty(advertEntity.Title))
                        advertEntity.Title = string.Concat(advertEntity.Car.Brand, " ", advertEntity.Car.Model);

                    await DbContext.Adverts.AddAsync(advertEntity);

                    if (advertVm.ImagesUploaded.Images is not null || advertVm.ImagesUploaded.Images.Count != 0)
                        await _imageService.SaveUploadedImagesAsync(advertVm.ImagesUploaded.Images, advertEntity.Id);

                }
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
                                                            .AsQueryable()
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

        public IEnumerable<UserAdvertVm> GetUserAdvertsAsQueryable(Expression<Func<Advert, bool>> filterExpressions)
        {
            try
            {
                var adverts = DbContext.Adverts.AsQueryable();
                adverts = adverts.Where(filterExpressions);
                
                return Mapper.Map<IEnumerable<UserAdvertVm>>(adverts);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return Enumerable.Empty<UserAdvertVm>();
            }
        }

        public async Task<bool> EndAdvertAsync(Guid advertId)
        {
            try
            {
                var advertToEnd = await DbContext.Adverts.FirstOrDefaultAsync(ad => ad.Id == advertId);

                if (advertToEnd == null)
                    throw new ArgumentNullException($"Brak ogłoszenia z podanym id: {advertId}");

                advertToEnd.ClosedAt = DateTime.Now;
                advertToEnd.IsActive = false;
                
                await DbContext.SaveChangesAsync();
                
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteAdvertAsync(Guid advertId)
        {
            try
            {
                var advertEntity = await DbContext.Adverts.FirstOrDefaultAsync(ad => ad.Id == advertId);
                
                if (advertEntity == null)
                    throw new ArgumentNullException($"Nie ma ogłoszenia z tym numerem id {advertId}");

                var carEntity = await DbContext.Cars.FirstOrDefaultAsync(car => car.Advert.Id == advertId);

                
                DbContext.Remove(advertEntity);
                DbContext.CarEquipments.RemoveRange(carEntity.CarEquipment);
                DbContext.Remove(carEntity);              
                
                _imageService.DeleteSavedImages(advertId);

                await DbContext.SaveChangesAsync();

                return true;

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<AddOrEditAdvertVm> GetAdvertToEditAsync(Guid advertId)
        {
            try
            {
                var advert = await DbContext.Adverts.FirstOrDefaultAsync(ad => ad.Id == advertId);

                var addOrEditAdvert = new AddOrEditAdvertVm {
                    Advert = Mapper.Map<AdvertVm>(advert),
                    Car = Mapper.Map<CarVm>(advert.Car),
                    Equipments = new CheckedEquVm { Checked = advert.Car.CarEquipment.Select(ad => ad.EquipmentId) },
                    ImagesUploaded = new ImageUploadedVm { ImagesSaved = Mapper.Map<IEnumerable<ImageVm>>(advert.Images)}
                };
                return addOrEditAdvert;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<UserAdvertVm> GetAdvertAsync(Guid advertId)
        {
            try
            {
                var advertEntity = await DbContext.Adverts.FirstOrDefaultAsync(ad => ad.Id == advertId);
                
                if (advertEntity is null)
                {
                    throw new ArgumentException($"Nie ma ogłoszenia z tym id: {advertId}");
                }

                var userAdvert = Mapper.Map<UserAdvertVm>(advertEntity);
                var equIds = advertEntity.Car.CarEquipment
                    .Select(equId => equId.EquipmentId)
                    .AsEnumerable();
                userAdvert.Equipment = await _equipmentService.GetEquipmentsAsync(equIds);
                return userAdvert;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task FollowAdvert(Guid advertId, int userId)
        {
            try
            {
                await DbContext.FollowAdverts.AddAsync(new FollowAdvert { 
                    AdvertId = advertId,
                    UserId = userId
                });

                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
            }
        }

        public async Task<bool> IsFollowedAdvert(Guid advertId, int userId)
        {
            try
            {
                var followed = await DbContext.FollowAdverts.FirstOrDefaultAsync(followed => followed.AdvertId == advertId && followed.UserId == userId);
                return followed is not null;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return false;
            }
        }
        public async Task UnfollowAdvert(Guid advertId, int userId)
        {
            try
            {
                var followAdvertEntity = await DbContext.FollowAdverts.FirstOrDefaultAsync(
                    follow =>
                    follow.AdvertId == advertId &&
                    follow.UserId == userId);
                DbContext.FollowAdverts.Remove(followAdvertEntity);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
            }
        }

        public async Task<IEnumerable<UserAdvertVm>> GetFollowAdvertsAsync(IEnumerable<Guid> followIds, int? userId)
        {
            try
            {
                List<Advert> followedAdverts;
                //for logged users
                if (userId.HasValue)
                {
                    var followedIds = await DbContext.FollowAdverts
                        .Where(fl => fl.UserId == userId)
                        .Select(fl => fl.AdvertId)
                        .ToListAsync();
                    followedAdverts = await DbContext.Adverts
                        .Where(ad => followedIds.Contains(ad.Id))
                        .ToListAsync();                   
                }
                else
                {
                    followedAdverts = await DbContext.Adverts
                       .Where(ad => followIds.Contains(ad.Id))
                       .ToListAsync();
                }
                return Mapper.Map<IEnumerable<UserAdvertVm>>(followedAdverts);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public IQueryable<Advert> GetAdverts(Expression<Func<Advert, bool>> filterExpressions = null)
        {
            try
            {
                var adverts = DbContext.Adverts.Where(filterExpressions ?? (ad => ad.IsActive));
                return adverts;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return DbContext.Adverts.DefaultIfEmpty();
            }
        }

        private void UpdateAdvert(Advert advertToUpdate, Advert updatedAdvert)
        {
            try
            {
                advertToUpdate.CreatedAt = DateTime.Now;
                advertToUpdate.Brutto = updatedAdvert.Brutto;
                advertToUpdate.Price = updatedAdvert.Price;
                advertToUpdate.Currency = updatedAdvert.Currency;
                advertToUpdate.Title = string.IsNullOrEmpty(updatedAdvert.Title) ? string.Concat(advertToUpdate.Car.Brand, " ", advertToUpdate.Car.Model) : updatedAdvert.Title;
                advertToUpdate.Localization = updatedAdvert.Localization;
                advertToUpdate.Description = updatedAdvert.Description;
                advertToUpdate.ToNegotiate = updatedAdvert.ToNegotiate;
                advertToUpdate.VAT = updatedAdvert.VAT;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
            }
        }
    }
}
