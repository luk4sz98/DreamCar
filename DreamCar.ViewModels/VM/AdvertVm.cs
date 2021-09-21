using DreamCar.Model.DataModels.Enums;
using System.ComponentModel.DataAnnotations;

namespace DreamCar.ViewModels.VM
{
    public class AdvertVm
    {
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Tytuł nie może zawierać specjalnych znaków")]
        [DataType(DataType.Text)]
        [StringLength(200, ErrorMessage = "Tytuł musi zawierać co najmniej {2} oraz maksymalnie {1} znaków", MinimumLength = 10)]
        public string Title { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9\s,]+$", ErrorMessage = "Opis nie może zawierać specjalnych znaków")]
        [DataType(DataType.MultilineText)]
        [StringLength(4096, ErrorMessage = "Opis musi zawierać co najmniej {2} oraz maksymalnie {1} znaków", MinimumLength = 10)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Cena jest wymagana")]
        [StringLength(12, ErrorMessage = "Cena musi zawierać co najmniej {2} oraz maksymalnie {1} cyfr", MinimumLength = 6)]
        public string Price { get; set; }

        [Required(ErrorMessage = "Musisz podać nazwę miasta")]
        [StringLength(50, ErrorMessage = "Miasto musi składać się co najmniej z {2} i maksymalnie {1} znaków.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-ZżźćńółęąśŻŹĆĄŚĘŁÓŃ0-9-{0,1}]+$", ErrorMessage = "Miasto może zawierać wyłącznie litery lub cyfry jako kod pocztowy")]
        public string Localization { get; set; }

        [Required(ErrorMessage = "Musisz podać adres email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Niepoprawny adres email")]
        [EmailAddress(ErrorMessage = "Niepoprawny adres email")]
        public string Email { get; set; }

        [StringLength(16, ErrorMessage = "Nr telefonu musi składać się z co najmniej {2} i maksymalnie {1} znaków.", MinimumLength = 11)]
        [RegularExpression(@"^\([0-9]{2}\)\s[0-9]{3}-[0-9]{3}-[0-9]{3}$", ErrorMessage = "Podaj prawidłowy numer")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public Currency Currency { get; set; }
        public bool VAT { get; set; }
        public bool Netto { get; set; }
        public bool ToNegotiate { get; set; }
    }
}
