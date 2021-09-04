using DreamCar.ViewModels.VM;
using System.Threading.Tasks;

namespace DreamCar.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ContactDetailsVm> GetAccountDetails(int userId);
        Task<bool> SaveContactDetails(ContactDetailsVm contactDetailsVm);
    }
}
