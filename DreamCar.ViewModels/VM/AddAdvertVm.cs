using System.Collections.Generic;

namespace DreamCar.ViewModels.VM
{
    public class AddAdvertVm
    {
        public CarVm Car { get; set; }
        public IEnumerable<EquipmentVm> Equipment { get; set; }
    }
}
