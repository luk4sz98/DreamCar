using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using DreamCar.Model.DataModels;
using Microsoft.AspNetCore.Identity;
using DreamCar.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using DreamCar.ViewModels.VM;
using Microsoft.Extensions.Options;
using DreamCar.Services.Services;
using Microsoft.AspNetCore.Hosting;

namespace DreamCar.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSenderService _emailSender;
        private readonly IReCaptchaService _reCaptcha;
        private readonly IWebHostEnvironment _hostingEnviroment;

        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSenderService emailSender,
            IReCaptchaService reCaptcha,
            IWebHostEnvironment hostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _reCaptcha = reCaptcha;
            _hostingEnviroment = hostEnvironment;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Musisz podać imię")]
            [StringLength(20, ErrorMessage = "Imię musi składać się co najmniej z {2} i maksymalnie {1} znaków.", MinimumLength = 3)]
            [DataType(DataType.Text)]
            [Display(Name = "First name")]
            [RegularExpression(@"^[a-zA-ZżźćńółęąśŻŹĆĄŚĘŁÓŃ]+$", ErrorMessage = "Imie może zawierać wyłącznie litery")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Musisz podać nazwisko")]
            [StringLength(20, ErrorMessage = "Nazwisko musi składać się co najmniej z {2} i maksymalnie {1} znaków.", MinimumLength = 3)]
            [DataType(DataType.Text)]
            [Display(Name = "Last name")]
            [RegularExpression(@"^[a-zA-ZżźćńółęąśŻŹĆĄŚĘŁÓŃ]+$", ErrorMessage = "Nazwisko może zawierać wyłącznie litery")]
            public string LastName { get; set; }
            
            [Required(ErrorMessage = "Musisz podać adres email")]
            [EmailAddress(ErrorMessage = "Podaj prawidłowy adres email")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Musisz podać hasło")]
            [StringLength(100, ErrorMessage = "Hasło musi składać się co najmniej z {2} i maksymalnie {1} znaków.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Hasło")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Musisz potwierdzić hasło")]
            [DataType(DataType.Password)]
            [Display(Name = "Potwierdź hasło")]
            [Compare("Password", ErrorMessage = "Musisz podać te same hasła")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                if (!Request.Form.ContainsKey("g-recaptcha-response")) return Page();
                var captcha = Request.Form["g-recaptcha-response"].ToString();
                if (!await _reCaptcha.IsValid(captcha)) return Page();
                var user = new User { FirstName = Input.FirstName, LastName = Input.LastName, UserName = Input.Email, Email = Input.Email, RegistrationDate = DateTime.Now };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    //By default, new user has User role.
                    await _userManager.AddToRoleAsync(user, "User");
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code, returnUrl },
                        protocol: Request.Scheme);

                    var emailBody = FileService.ReadFile(pathToFile: _hostingEnviroment.WebRootPath + @"\assets\templates\emailConfirmationTemplate.html");
                    emailBody = emailBody.Replace("{{ConfirmationLink}}", HtmlEncoder.Default.Encode(callbackUrl));

                    var emailVm = new EmailVm()
                    {
                        Body = emailBody,
                        Recipient = Input.Email,
                        SenderEmail = "dream.car.inzynier@gmail.com",
                        SenderName = "Dream Car",
                        Subject = "Potwierdzenie rejestracji konta"
                    };

                    await _emailSender.SendEmailAsync(emailVm);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
