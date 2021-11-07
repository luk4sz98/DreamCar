using AutoMapper;
using DreamCar.DAL.EF;
using DreamCar.Model.DataModels;
using DreamCar.Services.Interfaces;
using DreamCar.ViewModels.VM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DreamCar.Web.Controllers
{
    [Authorize(Roles = "Admin, Mod, User")]
    public class ImageController : BaseController
    {
        private readonly IImageService _imageService;
        private readonly ApplicationDbContext _dbContext;

        public ImageController(
            ILogger logger,
            IMapper mapper,
            IImageService imageService,
            UserManager<User> userManager,
            ApplicationDbContext dbContext
        ) : base(logger, mapper, userManager)
        {
            _imageService = imageService;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> DeleteImage(Guid advertId, string imageName)
        {
            try
            {
                await _imageService.DeleteImage(advertId, imageName);
                
                var advert = await _dbContext.Adverts.FirstOrDefaultAsync(ad => ad.Id == advertId);
                var userImages = new ImageUploadedVm { ImagesSaved = Mapper.Map<IEnumerable<ImageVm>>(advert.Images) };
                ViewBag.advertId = advert.Id;
                return PartialView("_CarImagesUploadPartial", userImages);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }
    }
}
