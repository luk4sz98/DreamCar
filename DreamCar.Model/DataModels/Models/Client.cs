using System.Collections.Generic;


namespace DreamCar.Model.DataModels
{
    public class Client : User
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public virtual IEnumerable<Advert> Adverts { get; set; }
        public virtual IEnumerable<FollowAdvert> FollowAdverts { get; set; }
        public virtual IEnumerable<Message> Messages { get; set; }
    }
}
