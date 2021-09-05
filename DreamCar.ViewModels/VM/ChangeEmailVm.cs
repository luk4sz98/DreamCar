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
    }
}
