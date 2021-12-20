using AutoMapper;
using Microsoft.Extensions.Logging;
using DreamCar.Model.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DreamCar.Services.Interfaces;
using System.Threading.Tasks;
using DreamCar.ViewModels.VM;
using DreamCar.Services.Services.Export;

namespace DreamCar.Web.Controllers
{
    [Authorize(Roles = "Admin, Mod, User")]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IReCaptchaService _reCaptcha;

        public AccountController(
                UserManager<User> userManager,
                IAccountService accountService,
                ILogger logger,
                IMapper mapper,
                IReCaptchaService reCaptcha
            ) : base(logger, mapper, userManager)
        {
            _accountService = accountService;
            _reCaptcha = reCaptcha;
        }

        public async Task<IActionResult> Index()
        {
            var user = await UserManager.GetUserAsync(User);
            ViewBag.contactDetails = await _accountService.GetAccountDetailsAsync(user.Id);
            ViewBag.userId = user.Id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveNewContactData(ContactDetailsVm contactDetailsVm)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            var result = await _accountService.SaveContactDetailsAsync(contactDetailsVm);
            if (result)
            {
                TempData["SaveContactData"] = true;
                return RedirectToAction("Index");
            }
            TempData["ErrorAlert"] = true;
            return RedirectToAction("Index");
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

            var (result, error) = await _accountService.ChangePasswordAsync(changePasswordVm);
            if (result)
            {
                TempData["ChangePassword"] = true;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, error);
            TempData["ErrorChangePassword"] = true;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeEmail(ChangeEmailVm changeEmailVm)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            var (result, error) = await _accountService.ChangeEmailAsync(changeEmailVm);
            if (result)
            {
                TempData["EmailChanged"] = true;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, error);
            TempData["EmailChanged"] = false;
            TempData["ErrorMessage"] = error;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmailChange(string userId, string email, string code)
        {
            if (userId == null || email == null || code == null)
            {
                return RedirectToAction("Index");
            }

            var (result, error) = await _accountService.ConfirmChangeEmailAsync(userId, email, code);
            if (result)
            {
                TempData["EmailChangedConfirm"] = true;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, error);
            TempData["EmailChangedConfirm"] = false;
            TempData["ErrorMessage"] = error;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DownloadPersonalData(DownloadDeletePersonalDataVm personalDataVm)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");
            var (result, error) = await _accountService.GetPersonalDataAsync(personalDataVm);

            if (result != null) return new ExportToCsvService(result, "PersonalData.csv");
            TempData["DownloadPersonalData"] = false;
            TempData["ErrorDownloadPersonalData"] = error;
            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount(DownloadDeletePersonalDataVm personalDataVm)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");
            var (result, error) = await _accountService.DeleteAccountAsync(personalDataVm);

            if (result)
            {
                TempData["DeletedAccountSuccess"] = true;
                return Json(new { redirectToUrl = Url.Action("Index", "Home") });
            }

            ModelState.AddModelError(string.Empty, error);
            TempData["DeletedAccountFailedMessage"] = error;
            return Json(new { redirectToUrl = Url.Action("Index", "Account") });
        }
    }
}
