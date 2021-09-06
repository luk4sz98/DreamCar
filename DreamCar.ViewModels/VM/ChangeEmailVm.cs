using System.ComponentModel.DataAnnotations;

namespace DreamCar.ViewModels.VM
{
    public class ChangeEmailVm
    {
        [Required]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Jeśli chcesz ustawić nowy adres email, musisz go podać")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Podaj prawidłowy adres email")]
        public string NewEmail { get; set; }

        [Required(ErrorMessage = "W celu potwierdzenia zmiany maila, podaj swoje obecne hasło")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Hasło musi składać się co najmniej z {2} i maksymalnie {1} znaków.", MinimumLength = 6)]
        public string CurrentPassword { get; set; }
    }
}
