using System.ComponentModel.DataAnnotations;

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
