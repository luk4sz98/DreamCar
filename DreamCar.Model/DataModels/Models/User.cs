using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DreamCar.Model.DataModels
{
    public class User : IdentityUser<int>
    {
        [PersonalData]
        [Display(Name = "Imie")]
        public string FirstName { get; set; }

        [PersonalData]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [PersonalData]
        [Display(Name = "Data rejestracji")]
        public DateTime RegistrationDate { get; set; }

        [PersonalData]
        [Display(Name = "Kraj")]
        public string Country { get; set; }

        [PersonalData]
        [Display(Name = "Miasto")]
        public string City { get; set; }

        [PersonalData]
        [Display(Name = "Kod pocztowy")]
        public string ZipCode { get; set; }

        [Display(Name = "Nr telefonu")]
        public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }

        [Display(Name = "Potwierdzony nr tel")]
        public override bool PhoneNumberConfirmed { get => base.PhoneNumberConfirmed; set => base.PhoneNumberConfirmed = value; }

        [Display(Name = "Uwierzytelnianie dwuskładnikowe")]
        public override bool TwoFactorEnabled { get => base.TwoFactorEnabled; set => base.TwoFactorEnabled = value; }

        [Display(Name = "Potwierdzony email")]
        public override bool EmailConfirmed { get => base.EmailConfirmed; set => base.EmailConfirmed = value; }

        [Display(Name = "Nazwa użytkownika")]
        public override string UserName { get => base.UserName; set => base.UserName = value; }

        [Display(Name = "Email")]
        public override string Email { get => base.Email; set => base.Email = value; }

        [Display(Name = "Id")]
        public override int Id { get => base.Id; set => base.Id = value; }

        public virtual IEnumerable<Advert> Adverts { get; set; }
        public virtual IEnumerable<FollowAdvert> FollowAdverts { get; set; }
        public virtual IEnumerable<Message> Messages { get; set; }
    }
}
