using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamCar.Model.DataModels.Enums
{
    public enum FuelType
    {
        Petrol,
        LPG,
        [Display(Name="Petrol+LPG")]
        PetrolLPG,
        ON,
        Hybrid,
        Electric
    }
}
