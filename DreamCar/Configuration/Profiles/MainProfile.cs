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

            CreateMap<User, ContactDetailsVm>()
                .ForMember(dest => dest.UserId, y => y.MapFrom(src => src.Id));
            
            CreateMap<ContactDetailsVm, User>()
                .ForMember(dest => dest.Id, y => y.MapFrom(src => src.UserId))
                .ForMember(dest => dest.City, y => y.MapFrom(src => src.City))
                .ForMember(dest => dest.Country, y => y.MapFrom(src => src.Country))
                .ForMember(dest => dest.ZipCode, y => y.MapFrom(src => src.ZipCode));

            CreateMap<Equipment, EquipmentVm>();

            CreateMap<CarVm, Car>();
            CreateMap<Car, CarVm>();
        }
    }
}
