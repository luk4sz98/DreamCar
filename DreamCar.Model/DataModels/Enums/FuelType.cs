using System.ComponentModel.DataAnnotations;

namespace DreamCar.Model.DataModels.Enums
{
    public enum FuelType
    {
        [Display(Name = "Benzyna")]
        Petrol,

        [Display(Name = "LPG")]
        LPG,

        [Display(Name="Benzyna + LPG")]
        PetrolLPG,
        
        [Display(Name = "Diesel")]
        ON,

        [Display(Name = "Hybryda")]
        Hybrid,

        [Display(Name = "Elektryk")]
        Electric
    }
}
