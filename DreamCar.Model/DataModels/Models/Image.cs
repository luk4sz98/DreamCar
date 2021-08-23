using System;
using System.ComponentModel.DataAnnotations;

namespace DreamCar.Model.DataModels
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        public string FileName { get; set; }
        public Guid AdvertId { get; set; }
        public virtual Advert Advert { get; set; }
    }
}
