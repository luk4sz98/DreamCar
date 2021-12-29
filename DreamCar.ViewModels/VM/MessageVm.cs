using System;
using System.ComponentModel.DataAnnotations;

namespace DreamCar.ViewModels.VM
{
    public class MessageVm
    {
        public int Id { get; set; }
        public int AdvertThreadId { get; set; }
        public int SenderId { get; set; }
        public int RecipientId { get; set; }

        [Required(ErrorMessage = "Wiadomość jest wymagana")]
        [DataType(DataType.MultilineText)]
        [StringLength(4096, ErrorMessage = "Wiadomość musi zawierać co najmniej {2} oraz maksymalnie {1} znaków", MinimumLength = 15)]
        public string Content { get; set; }
        public DateTime PostDate { get; set; }
    }
}