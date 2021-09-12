
using System.ComponentModel.DataAnnotations;

namespace DreamCar.Model.DataModels.Enums
{
    public enum DriveType
    {
        [Display(Name = "Na tylnie koła")]
        RWD,

        [Display(Name = "Na przednie koła")]
        FWD,

        [Display(Name = "4x4 stały")]
        AWDConst,

        [Display(Name = "4x4 dołączany ręcznie")]
        AWDManual,

        [Display(Name = "4x4 dołączany automatycznie")]
        AWDAutomat
    }
}
