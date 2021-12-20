using DreamCar.Model.DataModels;
using DreamCar.ViewModels.VM;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DreamCar.Services.Interfaces
{
    public interface ICarService
    {
        Task<Car> AddNewCarAsync(CarVm carVm, IEnumerable<int> carEqu);
        Task UpdateCar(Car carToUpdate, Car carUpdated, IEnumerable<int> carEqu);
    }
}
