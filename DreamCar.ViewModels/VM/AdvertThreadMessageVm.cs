using System;
using System.ComponentModel.DataAnnotations;

namespace DreamCar.ViewModels.VM
{
    public class AdvertThreadMessageVm
    {
        [Required]
        public Guid AdvertId { get; set; }

        [Required(ErrorMessage = "Temat jest wymagany")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Wiadomość jest wymagana")]
        [DataType(DataType.MultilineText)]
        [StringLength(4096, ErrorMessage = "Wiadomość musi zawierać co najmniej {2} oraz maksymalnie {1} znaków", MinimumLength = 10)]
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }

        [Required]
        public int SenderId { get; set; }

        [Required]
        public int RecipientId { get; set; }
    }
}
