using DreamCar.ViewModels.VM;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DreamCar.Services.Interfaces
{
    public interface IEquipmentService
    {
        Task<IEnumerable<EquipmentVm>> GetEquipmentsAsync();
    }
}
