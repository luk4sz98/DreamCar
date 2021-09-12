
using System.ComponentModel.DataAnnotations;

namespace DreamCar.Model.DataModels.Enums
{
    public enum BodyType
    {
        [Display(Name = "Kompakt")]
        Compact,

        [Display(Name = "Sedan")]
        Sedan,

        [Display(Name = "Kombi")]
        Combi,

        [Display(Name = "Kabriolet")]
        Cabriolet,

        [Display(Name = "SUV")]
        SUV,

        [Display(Name = "Coupe")]
        Coupe,

        [Display(Name = "Minivan")]
        Minivan,

        [Display(Name = "Auto miejskie")]
        CityCar,

        [Display(Name = "Auto małe")]
        SmallCar
    }
}
