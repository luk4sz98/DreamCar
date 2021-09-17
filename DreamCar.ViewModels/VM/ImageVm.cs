using DreamCar.ViewModels.Validators;
using Microsoft.AspNetCore.Http;

namespace DreamCar.ViewModels.VM
{
    public class ImageVm
    {
        [MinimumFileSizeValidator(1)]
        [MaximumFileSizeValidator(8)]
        [ValidFileTypeValidator("png", "jpg", "JPG")]
        public IFormFileCollection Image { get; set; }
    }
}
