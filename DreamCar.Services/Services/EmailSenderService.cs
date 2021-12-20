using AutoMapper;
using DreamCar.DAL.EF;
using DreamCar.Model.DataModels;
using DreamCar.Services.Interfaces;
using DreamCar.ViewModels.VM;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SendGrid;
using System;
using System.Threading.Tasks;
using SendGrid.Helpers.Mail;

namespace DreamCar.Services.Services
{
    public class EmailSenderService : BaseService, IEmailSenderService
    {
        private readonly SendGridClient _client;
        public EmailSenderService(
            ApplicationDbContext dbContext,
            IMapper mapper,
            ILogger logger,
            UserManager<User> userManager,
            SendGridClient sendGridClient) : base(dbContext, mapper, logger, userManager)
        {
            _client = sendGridClient;
        }

        public Task SendEmailAsync(EmailVm email)
        {
            try
            {
                if (email == null)
                    throw new ArgumentNullException($"Email can not be null");

                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(email.SenderEmail, email.SenderName),
                    Subject = email.Subject
                };

                if(email.Recipient != email.SenderEmail)
                {
                    msg.HtmlContent = email.Body;
                    msg.PlainTextContent = email.Body;
                } 
                else
                    msg.HtmlContent = "<p>" + email.Body + "</p>" + "</br>" + "<p>" + "Wiadomość wysłana przez użytkownika " + email.SenderName + " (adres e-mail: " + email.ResponseEmail + ")" + "</p>"; 

                msg.AddTo(new EmailAddress(email.Recipient));
                msg.SetClickTracking(false, false);

                return _client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
