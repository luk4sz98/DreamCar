using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using DreamCar.Model.DataModels;
using DreamCar.Services.Interfaces;
using DreamCar.ViewModels.VM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DreamCar.Web.Controllers
{
    public class MessageController : BaseController
    {
        private readonly IMessageService _messageService;
        public MessageController(ILogger logger, IMapper mapper, UserManager<User> userManager, IMessageService messageService) 
            : base(logger, mapper, userManager)
        {
            _messageService = messageService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Mod, User")]
        public async Task<IActionResult> CreateNewThread(AdvertThreadMessageVm threadMessage)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("GetAdvert", "Advert", new {advertId = threadMessage.AdvertId});
            }

            var result = await _messageService.CreateAdvertThread(threadMessage);

            switch (result)
            {
                case HttpStatusCode.BadRequest:
                case HttpStatusCode.InternalServerError:
                    TempData["Error"] = true;
                    return RedirectToAction("GetAdvert", "Advert", new {advertId = threadMessage.AdvertId});
                case HttpStatusCode.OK:
                    //TODO Create a view for messages
                    TempData["MessageSentSuccessfully"] = true;
                    return RedirectToAction("GetSellingAdvertThreads");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Mod, User")]
        [Route("Adverts/Threads/Selling")]
        public async Task<IActionResult> GetSellingAdvertThreads()
        {
            var userId = (await UserManager.GetUserAsync(User)).Id;
            var advertThreads = await _messageService.GetAdvertThreads(
                at => 
                    at.Advert.UserId == userId &&
                    at.CreatedById != userId &&
                    !at.Advert.IsActive);
            return View("AdvertThreadsSelling", advertThreads);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Mod, User")]
        [Route("Adverts/Threads/Buying")]
        public async Task<IActionResult> GetBuyingAdvertThreads()
        {
            var userId = (await UserManager.GetUserAsync(User)).Id;
            var advertThreads = await _messageService.GetAdvertThreads(
                at =>
                    at.Advert.UserId != userId &&
                    at.CreatedById == userId &&
                    !at.Advert.IsActive);
            return View("AdvertThreadsBuying", advertThreads);
        }
    }
}
