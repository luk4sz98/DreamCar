using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DreamCar.Model.DataModels
{
    public class AdvertThread
    {
        [Key]
        public int Id { get; set; }
        public string Subject { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public Guid AdvertId { get; set; }
        public virtual Advert Advert { get; set; }
        public virtual IEnumerable<Message> Messages { get; set; }

    }
}
