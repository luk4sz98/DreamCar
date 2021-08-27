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
using Newtonsoft.Json;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace DreamCar.Services.Services
{
    public class EmailSender : BaseService, IEmailSender
    {
        private readonly SendGridClient _client;
        private readonly IHostingEnvironment _hostingEnviroment;
        public EmailSender(
            ApplicationDbContext dbContext,
            IMapper mapper,
            ILogger logger,
            UserManager<User> userManager,
            IHostingEnvironment hostingEnvironment,
            SendGridClient sendGridClient) : base(dbContext, mapper, logger, userManager)
        {
            _client = sendGridClient;
            _hostingEnviroment = hostingEnvironment;
        }

        public Task SendEmailAsync(EmailVm email)
        {
            try
            {
                if (email == null)
                    throw new ArgumentNullException($"{email} can not be null");

                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(email.SenderEmail, email.SenderName),
                    Subject = email.Subject
                };

                if(email.Recipient != email.SenderEmail)
                {
                    string filePath = _hostingEnviroment.WebRootPath + @"\assets\templates\emailTemplate.html";
                    StreamReader reader = new StreamReader(filePath);
                    string textMail = reader.ReadToEnd();
                    reader.Close();
              
                    textMail = textMail.Replace("{{ConfirmationLink}}", email.Body);
                    msg.HtmlContent = textMail;
                    msg.Subject = "Potwierdzenie rejestracji konta";
                    msg.PlainTextContent = textMail;
                } else
                {
                    msg.HtmlContent = "<p>" + email.Body + "</p>" + "</br>" + "<p>" + "Wiadomość wysłana przez użytkownika " + email.SenderName + " (adres e-mail: " + email.ResponseEmail + ")" + "</p>"; 
                }

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

        private class TemplateData
        {
            [JsonProperty("ConfirmationLink")]
            public string ConfirmationLink { get; set; }
        }
    }
}
