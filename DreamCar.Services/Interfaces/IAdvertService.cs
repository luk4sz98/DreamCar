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
        Task<(bool, string)> AddOrEditAdvertAsync(AddOrEditAdvertVm advertVm);
        Task<IEnumerable<UserAdvertVm>> GetUserAdvertsAsync(Expression<Func<Advert, bool>> filterExpressions);
        IEnumerable<UserAdvertVm> GetUserAdvertsAsQueryable(Expression<Func<Advert, bool>> filterExpressions);
        Task<bool> EndAdvertAsync(Guid advertId);
        Task<bool> DeleteAdvertAsync(Guid advertId);
        Task<AddOrEditAdvertVm> GetAdvertToEditAsync(Guid advertId);
        Task<UserAdvertVm> GetAdvertAsync(Guid advertId);
        Task FollowAdvert(Guid advertId, int userId);
        Task<bool> IsFollowedAdvert(Guid advertId, int userId);
        Task UnfollowAdvert(Guid advertId, int userId);
        Task<IEnumerable<UserAdvertVm>> GetFollowAdvertsAsync(IEnumerable<Guid> followIds, int? userId);
    }
}
