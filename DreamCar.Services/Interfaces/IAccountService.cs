using DreamCar.ViewModels.VM;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DreamCar.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ContactDetailsVm> GetAccountDetailsAsync(int userId);
        Task<bool> SaveContactDetailsAsync(ContactDetailsVm contactDetailsVm);

        Task<(bool, string)> ChangePasswordAsync(ChangePasswordVm changePasswordVm);
        Task<(bool, string)> ChangeEmailAsync(ChangeEmailVm changeEmailVm);
        Task<(bool, string)> ConfirmChangeEmailAsync(string userId, string email, string code);

        Task<(Dictionary<string, string>, string)> GetPersonalDataAsync(DownloadDeletePersonalDataVm downloadDeletePersonalDataVm);
        Task<(bool, string)> DeleteAccountAsync(DownloadDeletePersonalDataVm personalDataVm);
    }
}
