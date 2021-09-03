using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using DreamCar.Model.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using DreamCar.Services.Interfaces;
using DreamCar.ViewModels.VM;
using DreamCar.Services.Services;
using Microsoft.AspNetCore.Hosting;

namespace DreamCar.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSenderService _emailSender;
        private readonly IWebHostEnvironment _hostingEnviroment;

        public ForgotPasswordModel(
            UserManager<User> userManager,
            IEmailSenderService emailSender,
            IWebHostEnvironment hostingEnviroment)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _hostingEnviroment = hostingEnviroment;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Podaj adres email, na który założyłeś konto")]
            [EmailAddress(ErrorMessage = "Podaj prawidłowy adres email")]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    ModelState.AddModelError(string.Empty,"Nie istnieje użytkownik o podanym adresie email, lub nie został on potwierdzony");
                    return Page();
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);


                var emailBody = FileService.ReadFile(pathToFile: _hostingEnviroment.WebRootPath + @"\assets\templates\emailPasswordResetTemplate.html");
                emailBody = emailBody.Replace("{{ResetLink}}", HtmlEncoder.Default.Encode(callbackUrl));

                var emailVm = new EmailVm()
                {
                    Body = emailBody,
                    Recipient = Input.Email,
                    SenderEmail = "dream.car.inzynier@gmail.com",
                    SenderName = "Dream Car",
                    Subject = "Resetowanie hasła"
                };

                await _emailSender.SendEmailAsync(emailVm);

                TempData["ResetPassword"] = true;
                return RedirectToPage("/Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Coś poszło nie tak, spróbuj ponownie");
                return Page();
            }
        }
    }
}
