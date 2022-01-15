using System;
using System.Linq;
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
    [Authorize(Roles = "Admin, Mod, User")]
    public class MessageController : BaseController
    {
        private readonly IMessageService _messageService;
        public MessageController(ILogger logger, IMapper mapper, UserManager<User> userManager, IMessageService messageService) 
            : base(logger, mapper, userManager)
        {
            _messageService = messageService;
        }

        [HttpPost]
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
                    TempData["MessageSentSuccessfully"] = true;
                    return RedirectToAction("GetSellingAdvertThreads");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpGet]
        [Route("Adverts/Threads/Selling")]
        public async Task<IActionResult> GetSellingAdvertThreads()
        {
            var userId = (await UserManager.GetUserAsync(User)).Id;
            var advertThreads = await _messageService.GetAdvertThreads(
                at => 
                    at.Advert.UserId == userId &&
                    at.CreatedById != userId &&
                    at.Advert.IsActive);
            return View("AdvertThreadsSelling", advertThreads);
        }

        [HttpGet]
        [Route("Adverts/Threads/Buying")]
        public async Task<IActionResult> GetBuyingAdvertThreads()
        {
            var userId = (await UserManager.GetUserAsync(User)).Id;
            var advertThreads = await _messageService.GetAdvertThreads(
                at =>
                    at.Advert.UserId != userId &&
                    at.CreatedById == userId &&
                    at.Advert.IsActive);
            return View("AdvertThreadsBuying", advertThreads);
        }

        [HttpGet]
        [Route("Adverts/Thread")]
        public async Task<IActionResult> GetAdvertThread(int id)
        {
            ViewBag.User = await UserManager.GetUserAsync(User);
            var advertThread = await _messageService.GetAdvertThread(id);
            return View("AdvertThread", advertThread);
        }

        [HttpGet]
        public async Task<IActionResult> SendMessage(int senderId, int recipientId, int advertThreadId, string message)
        {
            var messageVm = new MessageVm
            {
                AdvertThreadId = advertThreadId,
                Content = message,
                SenderId = senderId,
                RecipientId = recipientId,
                PostDate = DateTime.Now
            };
            ViewBag.User = await UserManager.GetUserAsync(User);
            var result = await _messageService.SendMessage(messageVm);
            return PartialView("_MessagesDataPartial", result.OrderBy(at => at.PostDate));
        }
    }
}
