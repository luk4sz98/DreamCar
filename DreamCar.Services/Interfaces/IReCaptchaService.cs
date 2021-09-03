using System.Threading.Tasks;

namespace DreamCar.Services.Interfaces
{
    public interface IReCaptchaService
    {
        Task<bool> IsValid(string captcha);
    }
}
