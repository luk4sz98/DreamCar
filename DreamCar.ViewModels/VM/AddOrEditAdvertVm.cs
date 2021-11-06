using System.Collections.Generic;

namespace DreamCar.ViewModels.VM
{
    public class AddOrEditAdvertVm
    {
        public CarVm Car { get; set; }
        public CheckedEquVm Equipments { get; set; }
        public ImageUploadedVm ImagesUploaded { get; set; }
        public AdvertVm Advert { get; set; }
    }
}
