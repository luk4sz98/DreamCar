
using System;

namespace DreamCar.Model.DataModels
{
    public class FollowAdvert
    {
        public int UserId { get; set; }
        public Guid AdvertId { get; set; }
        public virtual User User{ get; set; }
        public virtual Advert Advert { get; set; }
    }
}
