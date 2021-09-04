using AutoMapper;
using Microsoft.Extensions.Logging;
using DreamCar.Model.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DreamCar.Services.Interfaces;
using System.Threading.Tasks;
using DreamCar.ViewModels.VM;

namespace DreamCar.Web.Controllers
{
    [Authorize(Roles = "Admin, Mod, User")]
    public class AccountController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly IAccountService _accountService;

        public AccountController(
                UserManager<User> userManager,
                IAccountService accountService,
                ILogger logger,
                IMapper mapper
            ) : base(logger, mapper)
        {
            _userManager = userManager;
            _accountService = accountService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.contactDetails = await _accountService.GetAccountDetails(user.Id);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveNewContactData(ContactDetailsVm contactDetailsVm)
        {
            var result = await _accountService.SaveContactDetails(contactDetailsVm);
            if (result)
            {
                TempData["SaveContactData"] = true;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["SaveContactData"] = false;
                return RedirectToAction("Index");
            }
        }
    }
}
