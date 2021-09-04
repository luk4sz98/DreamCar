
using System.ComponentModel.DataAnnotations;

namespace DreamCar.ViewModels.VM
{
    public class ContactDetailsVm
    {
        [Required]
        public int UserId { get; set; }

        [StringLength(20, ErrorMessage = "Nazwa kraju musi składać się z co najmniej {2} i maksymalnie {1} liter", MinimumLength = 6)]
        [RegularExpression(@"^[a-zA-ZżźćńółęąśŻŹĆĄŚĘŁÓŃ]+$", ErrorMessage = "Nazwa kraju może zawierać wyłącznie litery")]
        public string Country { get; set; }

        [StringLength(6, ErrorMessage = "Kod pocztowy musi składać się z {2} znaków", MinimumLength = 6)]
        [RegularExpression("^[0-9]{2}-[0-9]{3}$", ErrorMessage = "Podaj prawidłowy kod pocztowy")]
        public string ZipCode { get; set; }

        [StringLength(16, ErrorMessage = "Nr telefonu musi składać się z co najmniej {2} i maksymalnie {1} znaków.", MinimumLength = 11)]
        [RegularExpression(@"^\([0-9]{2}\)\s[0-9]{3}-[0-9]{3}-[0-9]{3}$", ErrorMessage = "Podaj prawidłowy numer")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [StringLength(20, ErrorMessage = "Miasto musi składać się co najmniej z {2} i maksymalnie {1} znaków.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-ZżźćńółęąśŻŹĆĄŚĘŁÓŃ]+$", ErrorMessage = "Miasto może zawierać wyłącznie litery")]
        public string City { get; set; }
    }
}
