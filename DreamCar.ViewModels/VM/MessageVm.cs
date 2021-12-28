using System;

namespace DreamCar.ViewModels.VM
{
    public class MessageVm
    {
        public int Id { get; set; }
        public int AdvertThreadId { get; set; }
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public string Content { get; set; }
        public DateTime PostDate { get; set; }
    }
}