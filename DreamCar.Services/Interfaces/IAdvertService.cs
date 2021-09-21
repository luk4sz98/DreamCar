using DreamCar.ViewModels.VM;
using System.Threading.Tasks;

namespace DreamCar.Services.Interfaces
{
    public interface IAdvertService
    {
        Task<(bool, string)> AddNewAdvertAsync(AddAdvertVm advert);
    }
}
