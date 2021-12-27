using System;
using System.ComponentModel.DataAnnotations;

namespace DreamCar.Model.DataModels
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public int AdvertThreadId { get; set; }
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public string Content { get; set; }
        public DateTime PostDate { get; set; }
        public virtual AdvertThread AdvertThread { get; set; }
        public virtual User User { get; set; }
    }
}
