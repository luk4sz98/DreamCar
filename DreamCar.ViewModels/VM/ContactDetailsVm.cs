
using System.ComponentModel.DataAnnotations;

namespace DreamCar.ViewModels.VM
{
    public class ContactDetailsVm
    {
        [Required]
        public int UserId { get; set; }

        [StringLength(16, ErrorMessage = "Nr telefonu musi składać się z co najmniej {2} i maksymalnie {1} znaków.", MinimumLength = 11)]
        [RegularExpression(@"^\([0-9]{2}\)\s[0-9]{3}-[0-9]{3}-[0-9]{3}$", ErrorMessage = "Podaj prawidłowy numer")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [StringLength(50, ErrorMessage = "Adres musi składać się co najmniej z {2} i maksymalnie {1} znaków.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-ZżźćńółęąśŻŹĆĄŚĘŁÓŃ0-9-{0,1},{0,1}\s{0,1}]+$", ErrorMessage = "Adres może zawierać wyłącznie litery")]
        public string Address { get; set; }
    }
}
