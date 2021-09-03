using AutoMapper;
using DreamCar.Model.DataModels;
using DreamCar.ViewModels.VM;
using System;


namespace DreamCar.Web.Configuration.Profiles
{
    public class MainProfile : Profile
    {
        public MainProfile()
        {
            CreateMap<RegisterNewUserVm, User>()
                .ForMember(dest => dest.UserName, y => y.MapFrom(src => src.Email))
                .ForMember(dest => dest.RegistrationDate, y => y.MapFrom(src => DateTime.Now));

            CreateMap<Client, ContactDetailsVm>();
        }
    }
}
