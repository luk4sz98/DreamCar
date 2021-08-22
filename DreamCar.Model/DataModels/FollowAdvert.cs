
using System;
using System.ComponentModel.DataAnnotations;

namespace DreamCar.Model.DataModels
{
    public class FollowAdvert
    {
        public int UserId { get; set; }
        public Guid AdvertId { get; set; }
        public virtual Client Client { get; set; }
        public virtual Advert Advert { get; set; }
    }
}
