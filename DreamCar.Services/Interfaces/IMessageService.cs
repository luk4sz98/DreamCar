using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using DreamCar.Model.DataModels;
using DreamCar.ViewModels.VM;

namespace DreamCar.Services.Interfaces
{
    public interface IMessageService
    {
        Task<HttpStatusCode> CreateAdvertThread(AdvertThreadMessageVm advertThreadMessage);

        Task<IEnumerable<AdvertThreadVm>> GetAdvertThreads(Expression<Func<AdvertThread, bool>> filterExpressions);
    }
}
