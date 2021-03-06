using AutoMapper;
using DreamCar.Model.DataModels;
using DreamCar.Services.Interfaces;
using DreamCar.ViewModels.VM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DreamCar.Web.Controllers
{
    public class ContactController : BaseController
    {
        private readonly IEmailSenderService _emailSender;
        private readonly SignInManager<User> _signInManager;
        private readonly IReCaptchaService _reCaptcha;
        public ContactController(
                IEmailSenderService emailSender,
                UserManager<User> userManager,
                SignInManager<User> signInManager,
                IReCaptchaService reCaptcha,
                ILogger logger,
                IMapper mapper
            ) : base(logger, mapper, userManager)
        {
            _emailSender = emailSender;
            _signInManager = signInManager;
            _reCaptcha = reCaptcha;
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("Contact")]
        public IActionResult SendRequest()
        {
            if (!_signInManager.IsSignedIn(User)) return View();
            var user = UserManager.GetUserAsync(User).Result;
            ViewBag.Email = user.Email;
            ViewBag.User = $"{user.FirstName} {user.LastName}";
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendRequest(EmailVm email)
        {
            try
            {
                if (!Request.Form.ContainsKey("g-recaptcha-response")) 
                {
                    TempData["ErrorAlert"] = true;
                    return RedirectToAction("SendRequest");
                } 
                var captcha = Request.Form["g-recaptcha-response"].ToString();
                if (!await _reCaptcha.IsValid(captcha))
                {
                    TempData["ErrorAlert"] = true;
                    return RedirectToAction("SendRequest");
                }

                if(_signInManager.IsSignedIn(User))
                {
                    var user = await UserManager.GetUserAsync(User);
                    email.SenderName = $"{user.FirstName} {user.LastName}";
                    email.ResponseEmail = user.Email;
                }

                email.Recipient = email.SenderEmail = "dream.car.inzynier@gmail.com";
                await _emailSender.SendEmailAsync(email);
                
                TempData["MessageSent"] = true;
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                TempData["ErrorMailAlert"] = true;
                return RedirectToAction("SendRequest");
            }
        }
    }
}
