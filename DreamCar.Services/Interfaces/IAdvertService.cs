using DreamCar.ViewModels.VM;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DreamCar.Services.Interfaces
{
    public interface IAdvertService
    {
        Task<(bool, string)> AddNewAdvertAsync(AddAdvertVm advert);
        Task<IEnumerable<UserAdvertVm>> GetUserAdvertsAsync(int userId);
        IEnumerable<UserAdvertVm> GetUserAdvertsAsQueryable(string filterValue);
    }
}
