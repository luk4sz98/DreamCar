using AutoMapper;
using DreamCar.Model.DataModels;
using DreamCar.ViewModels.VM;
using System;
using System.Globalization;

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
                .ForMember(dest => dest.Address, y => y.MapFrom(src => src.Address));

            CreateMap<Equipment, EquipmentVm>();

            CreateMap<CarVm, Car>()
                .ForMember(dest => dest.GuaranteePeriod, y => y.MapFrom(
                    src => (string.IsNullOrEmpty(src.DayGuaranteePeriod) ||
                            string.IsNullOrEmpty(src.MonthGuaranteePeriod) ||
                            string.IsNullOrEmpty(src.YearGuaranteePeriod))
                            ? $"{src.DayGuaranteePeriod}-{src.MonthGuaranteePeriod}-{src.YearGuaranteePeriod}" : src.GuaranteePeriodMileage))
                .ForMember(dest => dest.CO2Emission, y => y.MapFrom(src => Convert.ToUInt16(src.CO2Emission)))
                .ForMember(dest => dest.Power, y => y.MapFrom(src => Convert.ToUInt16(src.Power)))
                .ForMember(dest => dest.EngineCapacity, y => y.MapFrom(src => Convert.ToUInt16(src.EngineCapacity)))
                .ForMember(dest => dest.Mileage, y => y.MapFrom(src => Convert.ToInt32(src.Mileage)));

            CreateMap<Car, CarVm>()
                .ForMember(dest => dest.CO2Emission, y => y.MapFrom(src => src.CO2Emission.ToString()))
                .ForMember(dest => dest.Power, y => y.MapFrom(src => src.Power.ToString()))
                .ForMember(dest => dest.EngineCapacity, y => y.MapFrom(src => src.EngineCapacity.ToString()))
                .ForMember(dest => dest.Mileage, y => y.MapFrom(src => src.Mileage.ToString()));

            CreateMap<AdvertVm, Advert>()
                .ForMember(dest => dest.Price, y => y.MapFrom(
                    src => src.Brutto ? decimal.Multiply(Convert.ToDecimal(src.Price, CultureInfo.InvariantCulture), 1.23m) :
                    Convert.ToDecimal(src.Price, CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.Id, y => y.MapFrom(src => src.AdvertId))
                .ForMember(dest => dest.Localization, y => y.MapFrom(src => src.City));
            CreateMap<Advert, AdvertVm>()
                .ForMember(dest => dest.AdvertId, y => y.MapFrom(src => src.Id))
                .ForMember(dest => dest.Voivodeship, y => y.MapFrom(src => src.Localization.Substring(src.Localization.LastIndexOf(',')+2)))
                .ForMember(dest => dest.City, y => y.MapFrom(src => src.Localization))
                .ForMember(dest => dest.Price, y => y.MapFrom(src => src.Brutto
                    ? decimal.Divide(src.Price, 1.23m) :
                    src.Price));

            CreateMap<Image, ImageVm>()
                .ForMember(dest => dest.Name, y => y.MapFrom(src => src.FileName));

            CreateMap<User, UserVm>();

            CreateMap<Advert, UserAdvertVm>()
                .ForMember(dest => dest.Images, y => y.MapFrom(src => src.Images))
                .ForMember(dest => dest.Car, y => y.MapFrom(src => src.Car))
                .ForMember(dest => dest.User, y => y.MapFrom(src => src.User));

            CreateMap<Car, Car>();

            CreateMap<AdvertThreadMessageVm, AdvertThread>()
                .ForMember(dest => dest.AdvertId, y => y.MapFrom(src => src.AdvertId))
                .ForMember(dest => dest.Subject, y => y.MapFrom(src => src.Subject))
                .ForMember(dest => dest.CreatedAt, y => y.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.CreatedBy, y => y.MapFrom(src => src.SenderId));
            CreateMap<AdvertThreadMessageVm, Message>()
                .ForMember(dest => dest.Content, y => y.MapFrom(src => src.Message))
                .ForMember(dest => dest.PostDate, y => y.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.RecipientId, y => y.MapFrom(src => src.RecipientId))
                .ForMember(dest => dest.SenderId, y => y.MapFrom(src => src.SenderId));
        }
    }
}
