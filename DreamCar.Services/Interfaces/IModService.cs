using System;
using System.Threading.Tasks;

namespace DreamCar.Services.Interfaces
{
    public interface IModService
    {
        Task<bool> AcceptOrNoAdvert(string reason, bool isGood, Guid advertId);
    }
}
