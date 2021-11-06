using DreamCar.ViewModels.Validators;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DreamCar.ViewModels.VM
{
    public class ImageUploadedVm
    {
        [ValidFileTypeValidator("png", "jpg", "JPG")]
        [CollectionLimitSizeValidator(10)]
        [MinimumFileSizeValidator(1)]
        [MaximumFileSizeValidator(8)]
        public IFormFileCollection Images { get; set; }
        public IEnumerable<ImageVm> ImagesSaved { get; set; }
    }
}
