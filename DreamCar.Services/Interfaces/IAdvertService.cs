using DreamCar.Model.DataModels;
using DreamCar.ViewModels.VM;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DreamCar.Services.Interfaces
{
    public interface IAdvertService
    {
        Task<(bool, string)> AddNewAdvertAsync(AddAdvertVm advert);
        Task<IEnumerable<UserAdvertVm>> GetUserAdvertsAsync(Expression<Func<Advert, bool>> filterExpressions);
        IEnumerable<UserAdvertVm> GetUserAdvertsAsQueryable(Expression<Func<Advert, bool>> filterExpressions);
        Task<bool> EndAdvertAsync(Guid advertId);
        Task<bool> DeleteAdvertAsync(Guid advertId);
    }
}
