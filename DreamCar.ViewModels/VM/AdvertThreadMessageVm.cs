using System;
using System.ComponentModel.DataAnnotations;

namespace DreamCar.ViewModels.VM
{
    public class AdvertThreadMessageVm
    {
        [Required]
        public Guid AdvertId { get; set; }

        [Required(ErrorMessage = "Temat jest wymagany")]
        [StringLength(30, ErrorMessage = "Temat nie może być dłuższy niż {1} oraz musi zawierać co najmniej {2} znaków", MinimumLength = 5)]
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
