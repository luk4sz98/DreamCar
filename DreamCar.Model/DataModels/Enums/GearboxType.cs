
using System.ComponentModel.DataAnnotations;

namespace DreamCar.Model.DataModels.Enums
{
    public enum GearboxType
    {
        [Display(Name = "Manualna")]
        Manual,
        
        [Display(Name = "Półautomatyczna")]
        SemiAutomatic,

        [Display(Name = "Automatyczna")]
        Autmatic
    }
}
