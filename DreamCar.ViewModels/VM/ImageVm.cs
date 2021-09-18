using DreamCar.ViewModels.Validators;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DreamCar.ViewModels.VM
{
    public class ImageVm
    {
        [ValidFileTypeValidator("png", "jpg", "JPG")]
        [CollectionLimitSizeValidator(10)]
        [MinimumFileSizeValidator(1)]
        [MaximumFileSizeValidator(8)]
        [Required(ErrorMessage = "Musisz podać co najmniej jedno zdjęcie")]
        public IFormFileCollection Image { get; set; }
    }
}
