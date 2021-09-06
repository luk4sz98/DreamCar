using System.ComponentModel.DataAnnotations;

namespace DreamCar.ViewModels.VM
{
    public class DownloadDeletePersonalDataVm
    {
        [Required]
        public int UserId { get; set; }
        
        [Required(ErrorMessage = "W celu potwierdzenia, musisz podać swoje hasło")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Hasło musi składać się co najmniej z {2} i maksymalnie {1} znaków.", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
