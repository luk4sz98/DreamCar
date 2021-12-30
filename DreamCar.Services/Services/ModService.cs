using AutoMapper;
using DreamCar.DAL.EF;
using DreamCar.Model.DataModels;
using DreamCar.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DreamCar.Services.Services.StaticClasses;
using DreamCar.ViewModels.VM;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DreamCar.Services.Services
{
    public class ModService : BaseService, IModService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IUrlHelper _url;
        private readonly IEmailSenderService _emailSender;
        private readonly IHttpContextAccessor _httpContext;
        public ModService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager,
            IWebHostEnvironment env, IUrlHelper url, IEmailSenderService emailSender, IHttpContextAccessor httpContext) 
            : base(dbContext, mapper, logger, userManager)
        {
            _env = env;
            _url = url;
            _emailSender = emailSender;
            _httpContext = httpContext;
        }

        public async Task<bool> AcceptOrNoAdvert(string reason, bool isGood, Guid advertId)
        {
            try
            {
                var advertAuthorEmail = (await DbContext.Adverts.FirstOrDefaultAsync(ad => ad.Id == advertId)).User.Email;
                var advertEntity = await DbContext.Adverts.FirstOrDefaultAsync(ad => ad.Id == advertId);
                if (advertEntity == null) return false;
                //akceptacja
                if (isGood)
                {
                    advertEntity.IsActive = true;
                    advertEntity.CheckedAt = DateTime.Now;
                    
                    await DbContext.SaveChangesAsync();
                    await SendEmail(advertId, advertAuthorEmail);
                    
                    return true;
                }
                //brak akcpetacji
                advertEntity.CheckedAt = DateTime.Now;
                advertEntity.IsActive = false;
                
                await DbContext.SaveChangesAsync();
                await SendEmail(advertId, advertAuthorEmail, reason);
                
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return false;
            }
        }

        private async Task SendEmail(Guid advertId, string authorEmail, string reason = null)
        {
            var callbackUrl = _url.Action("GetAdvert", "Advert", new { advertId = advertId }, _httpContext.HttpContext?.Request.Scheme);
            var emailVm = new EmailVm
            {
                Recipient = authorEmail,
                SenderEmail = "dream.car.inzynier@gmail.com",
                SenderName = "Dream Car",
                Subject = "Akceptacja ogłoszenia"
            };
            var emailBody = FileService.ReadFile(_env.WebRootPath + @"\assets\templates\advertAcceptedOrNot.html");

            if (string.IsNullOrEmpty(reason))
            {
                
                emailBody = emailBody.Replace("{{advertLink}}", HtmlEncoder.Default.Encode(callbackUrl));
                emailBody = emailBody.Replace("{{title}}", HtmlEncoder.Default.Encode("Akceptacja ogłoszenia"));
                emailBody = emailBody.Replace("{{isAccepted}}", HtmlEncoder.Default.Encode("Ogłoszenie zaakceptowane"));
                emailBody = emailBody.Replace("{{message}}", HtmlEncoder.Default.Encode("Twoje ogłoszenie zostało zaakceptowane.\n Naciśnij przycisk by przejść do ogłoszenia"));

                emailVm.Body = emailBody;

                await _emailSender.SendEmailAsync(emailVm);
                return;
            }

            emailBody = emailBody.Replace("{{advertLink}}", HtmlEncoder.Default.Encode(callbackUrl));
            emailBody = emailBody.Replace("{{title}}", HtmlEncoder.Default.Encode("Brak akceptacji ogłoszenia"));
            emailBody = emailBody.Replace("{{isAccepted}}", HtmlEncoder.Default.Encode("Ogłoszenie niezaakcepotwane"));
            emailBody = emailBody.Replace("{{message}}", HtmlEncoder.Default.Encode($"Twoje ogłoszenie nie zostało zaakceptowane.\nPowód: {reason}\nNaciśnij by przejść do ogłoszenia"));
            emailVm.Body = emailBody;
            
            await _emailSender.SendEmailAsync(emailVm);
        }
    }
}
