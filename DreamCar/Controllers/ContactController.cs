using DreamCar.Model.DataModels;
using DreamCar.Services.Interfaces;
using DreamCar.ViewModels.VM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DreamCar.Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public ContactController(
            IEmailSender emailSender,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _emailSender = emailSender;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult SendRequest()
        {
            if(_signInManager.IsSignedIn(User)) {
                var user = _userManager.GetUserAsync(User).Result;
                ViewBag.Email = user.Email;
                ViewBag.User = $"{user.FirstName} {user.LastName}";
                return View();
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SendRequest(EmailVm email)
        {
            if(ModelState.IsValid)
            {
                email.Recipient = "dream.car.inzynier@gmail.com";
                email.SenderEmail = "dream.car.inzynier@gmail.com";
                await _emailSender.SendEmailAsync(email);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["ErrorMailAlert"] = true;
                return RedirectToAction("SendRequest");
            }

        }

    }
}
