using DreamCar.Model.DataModels.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DreamCar.Model.DataModels
{
    public class Advert
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Localization { get; set; }
        public int Visited { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public bool IsActive { get; set; }
        public decimal Price { get; set; }
        public Currency Currency { get; set; }
        public bool ToNegotiate { get; set; }
        public bool Brutto { get; set; }
        public bool VAT { get; set; }

        [ForeignKey("Car")]
        public int CarId { get; set; }
        public int UserId { get; set; }

        public virtual IEnumerable<Image> Images { get; set; }
        public virtual IEnumerable<AdvertThread> AdvertThreads { get; set; }
        public virtual IEnumerable<FollowAdvert> FollowAdverts { get; set; }
        public virtual Car Car { get; set; }
        public virtual User User { get; set; }

    }
}
