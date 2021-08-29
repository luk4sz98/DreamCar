using DreamCar.Model.DataModels;
using DreamCar.Services.Interfaces;
using DreamCar.ViewModels.VM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;

namespace DreamCar.Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger _logger;
        private readonly IReCaptcha _reCaptcha;
        public ContactController(
            IEmailSender emailSender,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger logger,
            IReCaptcha reCaptcha)
        {
            _emailSender = emailSender;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _reCaptcha = reCaptcha;
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
                    var user = await _userManager.GetUserAsync(User);
                    var userCredentials = new StringBuilder();
                    userCredentials.Append(user.FirstName);
                    userCredentials.Append(' ');
                    userCredentials.Append(user.LastName);
                    email.SenderName = userCredentials.ToString();
                    email.ResponseEmail = user.Email;
                }

                email.Recipient = email.SenderEmail = "dream.car.inzynier@gmail.com";
                await _emailSender.SendEmailAsync(email);
                
                TempData["MessageSent"] = true;
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                TempData["ErrorMailAlert"] = true;
                return RedirectToAction("SendRequest");
            }
        }
    }
}
