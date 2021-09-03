using System;
using System.ComponentModel.DataAnnotations;

namespace DreamCar.Model.DataModels
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        public int AdvertThreadId { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public DateTime PostDate { get; set; }
        public virtual AdvertThread AdvertThread { get; set; }
        public virtual User User { get; set; }
    }
}
