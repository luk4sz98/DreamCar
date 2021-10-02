using DreamCar.Model.DataModels;
using DreamCar.ViewModels.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamCar.Services.Interfaces
{
    public interface ICarService
    {
        Task<Car> AddNewCarAsync(CarVm carVm, IEnumerable<int> carEqu);
    }
}
