using System.ComponentModel.DataAnnotations;

namespace DreamCar.ViewModels.VM
{
    public class EmailVm
    {
        [Required]
        public string Sender { get; set; }
        [Required]
        public string Recipient { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }
    }
}
