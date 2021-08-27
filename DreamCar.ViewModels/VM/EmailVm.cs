using System.ComponentModel.DataAnnotations;

namespace DreamCar.ViewModels.VM
{
    public class EmailVm
    {
        [Required]
        [RegularExpression(@"^[a-zA-ZżźćńółęąśŻŹĆĄŚĘŁÓŃ\s-]+$", ErrorMessage = "Personalia mogą zawierać wyłącznie litery")]
        [StringLength(50, ErrorMessage = "Personalia muszą składać się co najmniej z {2} i maksymalnie {1} znaków.", MinimumLength = 5)]
        [DataType(DataType.Text)]
        public string SenderName { get; set; }
        
        [Required]
        [EmailAddress(ErrorMessage = "Podaj prawidłowy adres email.")]
        public string SenderEmail { get; set; }
        
        [EmailAddress]
        public string ResponseEmail { get; set; }

        [Required]
        [EmailAddress]
        public string Recipient { get; set; }
        
        [Required(ErrorMessage = "Musisz podać temat maila")]
        public string Subject { get; set; }
        
        [Required]
        [StringLength(200, ErrorMessage = "Wiadomość musi zawierać co najmniej 20 znaków", MinimumLength = 20)]
        public string Body { get; set; }
    }
}
