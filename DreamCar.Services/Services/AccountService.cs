using AutoMapper;
using DreamCar.DAL.EF;
using DreamCar.Model.DataModels;
using DreamCar.Services.Interfaces;
using DreamCar.ViewModels.VM;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace DreamCar.Services.Services
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IUrlHelper _url;
        private readonly IWebHostEnvironment _hostingEnviroment;
        private readonly IEmailSenderService _emailSender;
        private readonly IHttpContextAccessor _httpContext;
        public AccountService(
                ApplicationDbContext dbContext,
                IMapper mapper,
                ILogger logger,
                UserManager<User> userManager,
                SignInManager<User> signInManager,
                IUrlHelper url,
                IWebHostEnvironment hostEnvironment,
                IEmailSenderService emailSender,
                IHttpContextAccessor httpContextAccessor
            ) : base(dbContext, mapper, logger, userManager)
        {
            _signInManager = signInManager;
            _url = url;
            _hostingEnviroment = hostEnvironment;
            _emailSender = emailSender;
            _httpContext = httpContextAccessor;
        }

        public async Task<(bool, string)> ChangePasswordAsync(ChangePasswordVm changePasswordVm)
        {
            try
            {
                var user = await DbContext.Users.OfType<User>().FirstOrDefaultAsync(user => user.Id == changePasswordVm.UserId);
                if (user == null)
                    throw new ArgumentNullException($"Nie ma użytkownika z tym id - {changePasswordVm.UserId}");

                var changePasswordResult = await UserManager.ChangePasswordAsync(user, changePasswordVm.OldPassword, changePasswordVm.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    var errorDescription = new StringBuilder();
                    foreach (var error in changePasswordResult.Errors)
                    {
                        errorDescription.Append(error + "\n");
                    }
                    throw new Exception(errorDescription.ToString());
                }

                await _signInManager.RefreshSignInAsync(user);

                return (true, "");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return (false, ex.Message);
            }
        }

        public async Task<ContactDetailsVm> GetAccountDetailsAsync(int userId)
        {
            try
            {
                var userEntity = await DbContext.Users.OfType<User>().FirstOrDefaultAsync(user => user.Id == userId);
                if (userEntity == null)
                    throw new ArgumentNullException($"Nie ma użytkownika z tym id - {userId}");
                var contactDetailVm = Mapper.Map<ContactDetailsVm>(userEntity);
                return contactDetailVm;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<bool> SaveContactDetailsAsync(ContactDetailsVm contactDetailsVm)
        {
            try
            {
                var userEnity = await DbContext.Users.FirstOrDefaultAsync(user => user.Id == contactDetailsVm.UserId);
                
                if (userEnity == null)
                    throw new ArgumentNullException($"Nie ma użytkownika z tym id - {contactDetailsVm.UserId}");

                userEnity.Address = contactDetailsVm.Address;
                userEnity.PhoneNumber = contactDetailsVm.PhoneNumber;
           
                await DbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<(bool, string)> ChangeEmailAsync(ChangeEmailVm changeEmailVm)
        {
            try
            {
                var user = await DbContext.Users.OfType<User>().FirstOrDefaultAsync(user => user.Id == changeEmailVm.UserId);
                if (user == null)
                    throw new ArgumentNullException($"Nie ma użytkownika z tym id - {user.Id}");

                if (user.Email == changeEmailVm.NewEmail)
                    return (false, "Adres email jest taki sam jak dotychczasowy");

                if (!await UserManager.CheckPasswordAsync(user, changeEmailVm.CurrentPassword))
                    return (false, "Niepoprawne hasło, spróbuj ponownie");

                var code = await UserManager.GenerateChangeEmailTokenAsync(user, changeEmailVm.NewEmail);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = _url.Action(
                    action: "ConfirmEmailChange",
                    controller: "Account",
                    values: new { userId = user.Id, email = changeEmailVm.NewEmail, code = code },
                    protocol: _httpContext.HttpContext.Request.Scheme);
                
                var emailBody = FileService.ReadFile(pathToFile: _hostingEnviroment.WebRootPath + @"\assets\templates\emailChangeConfirmationTemplate.html");
                emailBody = emailBody.Replace("{{ConfirmationLink}}", HtmlEncoder.Default.Encode(callbackUrl));
                
                var emailVm = new EmailVm()
                {
                    Body = emailBody,
                    Recipient = changeEmailVm.NewEmail,
                    SenderEmail = "dream.car.inzynier@gmail.com",
                    SenderName = "Dream Car",
                    Subject = "Potwierdzenie zmiany adresu email"
                };

                await _emailSender.SendEmailAsync(emailVm);
                return (true, string.Empty);

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return (false, ex.ToString());
            }
        }
    
        public async Task<(bool, string)> ConfirmChangeEmailAsync(string userId, string email, string code)
        {
            try
            {
                var user = await DbContext.Users.OfType<User>().FirstOrDefaultAsync(user => user.Id == int.Parse(userId));
                if (user == null)
                    throw new Exception("Nie ma takiego użytkownika");

                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                var result = await UserManager.ChangeEmailAsync(user, email, code);

                if (!result.Succeeded)
                    return (false, "Błąd podczas zmiany adresu email");

                var setUserNameResult = await UserManager.SetUserNameAsync(user, email);

                await _signInManager.RefreshSignInAsync(user);
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return (false, ex.Message);
            }
        }

        public async Task<(Dictionary<string, string>, string)> GetPersonalDataAsync(DownloadDeletePersonalDataVm downloadDeletePersonalDataVm)
        {
            try
            {
                var user = await DbContext.Users.OfType<User>().FirstOrDefaultAsync(user => user.Id == downloadDeletePersonalDataVm.UserId);
                if (user == null)
                    throw new ArgumentNullException($"Nie ma użytkownika z tym id - {downloadDeletePersonalDataVm.UserId}");


                if (!await UserManager.CheckPasswordAsync(user, downloadDeletePersonalDataVm.Password))
                    return (null, "Niepoprawne hasło, spróbuj ponownie");

                var personalData = new Dictionary<string, string>();
                var personalDataProps = typeof(User).GetProperties().Where(
                                prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));

                foreach (var p in personalDataProps)
                {
                    personalData.Add(
                        p.GetCustomAttributes(typeof(DisplayAttribute), true).Cast<DisplayAttribute>().Single().Name,
                        p.GetValue(user)?.ToString() ?? "null"
                    );
                }

                var logins = await UserManager.GetLoginsAsync(user);
                foreach (var l in logins)
                {
                    personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
                }

                return (personalData, string.Empty);
               
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return (null, ex.Message);
            }
        }

        public async Task<(bool, string)> DeleteAccountAsync(DownloadDeletePersonalDataVm personalDataVm)
        {
            try
            {
                var user = await DbContext.Users.OfType<User>().FirstOrDefaultAsync(user => user.Id == personalDataVm.UserId);
                if (user == null)
                    throw new ArgumentNullException($"Nie ma użytkownika z tym id - {personalDataVm.UserId}");
                
                if (!await UserManager.CheckPasswordAsync(user, personalDataVm.Password))
                    throw new ArgumentException("Niepoprawne hasło, spróbuj ponownie");

                var result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                    throw new InvalidOperationException("Wystapił błąd podczas próby skasowania konta, spróbuj ponownie");

                await _signInManager.SignOutAsync();
                Logger.LogInformation($"Użytkownik z id {personalDataVm.UserId} usunął swoje konto");
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return (false, ex.Message);
            }
        }
    }
}
