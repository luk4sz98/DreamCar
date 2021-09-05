using System.ComponentModel.DataAnnotations;

namespace DreamCar.ViewModels.VM
{
    public class ChangePasswordVm
    {
        [Required]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Musisz podać swoje obecne hasło")]
        [StringLength(100, ErrorMessage = "Hasło musi składać się co najmniej z {2} i maksymalnie {1} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Obecne hasło")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Musisz podać nowe hasło")]
        [StringLength(100, ErrorMessage = "Hasło musi składać się co najmniej z {2} i maksymalnie {1} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nowe hasło")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Musisz potwierdzić nowe hasło")]
        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź nowe hasło")]
        [Compare("NewPassword", ErrorMessage = "Musisz podać te same hasła")]
        public string ConfirmPassword { get; set; }
    }
}
