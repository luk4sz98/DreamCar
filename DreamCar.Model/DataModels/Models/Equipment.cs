using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DreamCar.Model.DataModels
{
    public class Equipment
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }

        public virtual IEnumerable<CarEquipment> CarEquipment { get; set; }
    }
}
