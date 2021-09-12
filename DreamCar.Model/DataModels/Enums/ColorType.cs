
using System.ComponentModel.DataAnnotations;

namespace DreamCar.Model.DataModels.Enums
{
    public enum ColorType
    {
        [Display(Name = "Matowy")]
        Mat,

        [Display(Name = "Metaliczny")]
        Metalic,

        [Display(Name = "Perłowy")]
        Pearl,

        [Display(Name = "Specjalny")]
        Special
    }
}
