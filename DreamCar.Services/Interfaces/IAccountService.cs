using DreamCar.ViewModels.VM;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DreamCar.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ContactDetailsVm> GetAccountDetails(int userId);
        Task<bool> SaveContactDetails(ContactDetailsVm contactDetailsVm);

        Task<(bool, string)> ChangePassword(ChangePasswordVm changePasswordVm);
        Task<(bool, string)> ChangeEmailAsync(ChangeEmailVm changeEmailVm);
        Task<(bool, string)> ConfirmChangeEmailAsync(string userId, string email, string code);

        Task<(Dictionary<string, string>, string)> GetPersonalDataAsync(DownloadDeletePersonalDataVm downloadDeletePersonalDataVm);
    }
}
