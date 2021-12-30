using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DreamCar.Model.DataModels;
using DreamCar.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DreamCar.Web.Controllers
{
    [Authorize(Roles = "Admin, Mod")]
    public class ModController : BaseController
    {
        private readonly IAdvertService _advertService;
        private readonly IModService _modService;
        public ModController(ILogger logger, IMapper mapper, UserManager<User> userManager,
            IAdvertService advertService, IModService modService) : base(logger, mapper, userManager)
        {
            _advertService = advertService;
            _modService = modService;
        }

        [Route("Mod/Adverts/Pending")]
        public async Task<IActionResult> GetAdvertsToCheck(bool? result)
        {
            var pendingAdverts = 
                await _advertService.GetUserAdvertsAsync(ad => 
                    !ad.IsActive && 
                    ad.ClosedAt == null);
            if (result.HasValue)
            {
                if (result.Value) TempData["Success"] = true;
                else TempData["Error"] = true;
            } 
            return View("PendingAdverts", pendingAdverts.Where(ad =>
                                          ad.CheckedAt == null || (DateTime.Now - ad.CheckedAt.Value).TotalDays > 7));
        }

        public async Task<IActionResult> CheckAdvert(string reason, bool isGood, Guid advertId)
        {
            var result = await _modService.AcceptOrNoAdvert(reason, isGood, advertId);
            return RedirectToAction("GetAdvertsToCheck", new { result });
        }
    }
}
