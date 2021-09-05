using AutoMapper;
using DreamCar.DAL.EF;
using DreamCar.Model.DataModels;
using DreamCar.Services.Interfaces;
using DreamCar.ViewModels.VM;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamCar.Services.Services
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly SignInManager<User> _signInManager;
        public AccountService(
                ApplicationDbContext dbContext,
                IMapper mapper,
                ILogger logger,
                UserManager<User> userManager,
                SignInManager<User> signInManager
            ) : base(dbContext, mapper, logger, userManager)
        {
            _signInManager = signInManager;
        }

        public async Task<(bool, string)> ChangePassword(ChangePasswordVm changePasswordVm)
        {
            try
            {
                var user = await DbContext.Users.OfType<User>().FirstOrDefaultAsync(user => user.Id == changePasswordVm.UserId);
                if (user == null)
                    throw new ArgumentNullException($"Nie ma użytkownika z tym id - {changePasswordVm.UserId}");

                var changePasswordResult = await UserManager.ChangePasswordAsync(user, changePasswordVm.OldPassword, changePasswordVm.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    var errorDescription = new StringBuilder();
                    foreach (var error in changePasswordResult.Errors)
                    {
                        errorDescription.Append(error + "\n");
                    }
                    throw new Exception(errorDescription.ToString());
                }

                await _signInManager.RefreshSignInAsync(user);

                return (true, "");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return (false, ex.Message);
            }
        }

        public async Task<ContactDetailsVm> GetAccountDetails(int userId)
        {
            try
            {
                var userEntity = await DbContext.Users.OfType<User>().FirstOrDefaultAsync(user => user.Id == userId);
                if (userEntity == null)
                    throw new ArgumentNullException($"Nie ma użytkownika z tym id - {userId}");
                var contactDetailVm = Mapper.Map<ContactDetailsVm>(userEntity);
                return contactDetailVm;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<bool> SaveContactDetails(ContactDetailsVm contactDetailsVm)
        {
            try
            {
                var userEnity = await DbContext.Users.FirstOrDefaultAsync(user => user.Id == contactDetailsVm.UserId);
                
                if (userEnity == null)
                    throw new ArgumentNullException($"Nie ma użytkownika z tym id - {contactDetailsVm.UserId}");

                userEnity.Country = contactDetailsVm.Country;
                userEnity.City = contactDetailsVm.City;
                userEnity.PhoneNumber = contactDetailsVm.PhoneNumber;
                userEnity.ZipCode = contactDetailsVm.ZipCode;

                
                await DbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return false;
            }
        }
    }
}
