using System.Threading.Tasks;

namespace DreamCar.Services.Interfaces
{
    public interface IReCaptcha
    {
        Task<bool> IsValid(string captcha);
    }
}
