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
        private readonly IReCaptchaService _reCaptcha;

        public AccountController(
                UserManager<User> userManager,
                IAccountService accountService,
                ILogger logger,
                IMapper mapper,
                IReCaptchaService reCaptcha
            ) : base(logger, mapper)
        {
            _userManager = userManager;
            _accountService = accountService;
            _reCaptcha = reCaptcha;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.contactDetails = await _accountService.GetAccountDetails(user.Id);
            ViewBag.userId = user.Id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveNewContactData(ContactDetailsVm contactDetailsVm)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            var result = await _accountService.SaveContactDetails(contactDetailsVm);
            if (result)
            {
                TempData["SaveContactData"] = true;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorAlert"] = true;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVm changePasswordVm)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            if (!Request.Form.ContainsKey("g-recaptcha-response"))
            {
                TempData["ErrorAlert"] = true;
                return RedirectToAction("Index");
            }
            var captcha = Request.Form["g-recaptcha-response"].ToString();
            if (!await _reCaptcha.IsValid(captcha))
            {
                TempData["ErrorAlert"] = true;
                return RedirectToAction("Index");
            }

            var result = await _accountService.ChangePassword(changePasswordVm);
            if (result.Item1)
            {
                TempData["ChangePassword"] = true;
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Item2);
                TempData["ErrorChangePassword"] = true;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeEmail(ChangeEmailVm changeEmailVm)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            var result = await _accountService.ChangeEmailAsync(changeEmailVm);
            if (result.Item1)
            {
                TempData["EmailChanged"] = true;
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Item2);
                TempData["EmailChanged"] = false;
                TempData["ErrorMessage"] = result.Item2;
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmailChange(string userId, string email, string code)
        {
            if (userId == null || email == null || code == null)
            {
                return RedirectToAction("Index");
            }

            var result = await _accountService.ConfirmChangeEmailAsync(userId, email, code);
            if (result.Item1)
            {
                TempData["EmailChangedConfirm"] = true;
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Item2);
                TempData["EmailChangedConfirm"] = false;
                TempData["ErrorMessage"] = result.Item2;
                return RedirectToAction("Index");
            }
        }
    }
}
