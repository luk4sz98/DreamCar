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
using Microsoft.Extensions.Options;

namespace DreamCar.Services.Services
{
    public class EmailSender : BaseService, IEmailSender
    {
        private readonly SendGridClient _client;
        private readonly IOptions<AppSettings> _options;
        public EmailSender(
            ApplicationDbContext dbContext,
            IMapper mapper, 
            ILogger logger,
            IOptions<AppSettings> options,
            UserManager<User> userManager,
            SendGridClient sendGridClient) : base(dbContext, mapper, logger, userManager)
        {
            _client = sendGridClient;
            _options = options;
        }

        public Task SendEmailAsync(EmailVm email)
        {
            try
            {
                if (email == null)
                    throw new ArgumentNullException($"{email} can not be null");
                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(email.Sender, "Dream Car"),
                    Subject = email.Subject,
                    PlainTextContent = email.Body,
                    HtmlContent = email.Body
                };
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
