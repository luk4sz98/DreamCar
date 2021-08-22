using DreamCar.ViewModels.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamCar.Services.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailVm email);
    }
}
