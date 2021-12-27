using System.Net;
using System.Threading.Tasks;
using DreamCar.ViewModels.VM;

namespace DreamCar.Services.Interfaces
{
    public interface IMessageService
    {
        Task<HttpStatusCode> CreateAdvertThread(AdvertThreadMessageVm advertThreadMessage);
    }
}
