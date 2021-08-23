using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DreamCar.Model.DataModels
{
    public class CarEquipment
    {
        public int CarId { get; set; }
        public int EquipmentId { get; set; }
        public virtual Car Car { get; set; }
        public virtual Equipment Equipment { get; set; }
    }
}
