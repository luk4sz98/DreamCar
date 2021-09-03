using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace DreamCar.Model.DataModels
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public virtual IEnumerable<Advert> Adverts { get; set; }
        public virtual IEnumerable<FollowAdvert> FollowAdverts { get; set; }
        public virtual IEnumerable<Message> Messages { get; set; }
    }
}
