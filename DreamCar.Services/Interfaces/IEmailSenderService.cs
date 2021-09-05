using DreamCar.ViewModels.VM;
using System.Threading.Tasks;

namespace DreamCar.Services.Interfaces
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(EmailVm email);
    }
}
