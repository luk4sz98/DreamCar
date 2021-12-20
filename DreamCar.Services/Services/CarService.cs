using AutoMapper;
using DreamCar.DAL.EF;
using DreamCar.Model.DataModels;
using DreamCar.Services.Interfaces;
using DreamCar.ViewModels.VM;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DreamCar.Services.Services
{
    public class CarService : BaseService, ICarService
    {
        public CarService(
            ApplicationDbContext dbContext,
            IMapper mapper,
            ILogger logger,
            UserManager<User> userManager) : base(
                dbContext,
                mapper,
                logger,
                userManager)
        {
        }

        public async Task<Car> AddNewCarAsync(CarVm carVm,
                                              IEnumerable<int> carEqu)
        {
            try
            {
                var carEntity = Mapper.Map<Car>(carVm);
                
                await DbContext.Cars.AddAsync(carEntity);

                var carEquipments = new List<CarEquipment>();

                if (carEqu is not null)
                {
                    carEquipments.AddRange(carEqu.Select(equId => new CarEquipment {Car = carEntity, EquipmentId = equId}));
                }
              
                await DbContext.CarEquipments.AddRangeAsync(carEquipments);
                
                return carEntity;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task UpdateCar(Car carToUpdate, Car carUpdated, IEnumerable<int> carEqu)
        {
            try
            {
                #region carFields
                carToUpdate.Brand = carUpdated.Brand;
                carToUpdate.Model = carUpdated.Model;
                carToUpdate.Version = carUpdated.Version;
                carToUpdate.Color = carUpdated.Color;
                carToUpdate.ProductionYear = carUpdated.ProductionYear;
                carToUpdate.Mileage = carUpdated.Mileage;
                carToUpdate.VIN = carUpdated.VIN;
                carToUpdate.Doors = carUpdated.Doors;
                carToUpdate.Seats = carUpdated.Seats;
                carToUpdate.OriginCountry = carUpdated.OriginCountry;
                carToUpdate.IsImported = carUpdated.IsImported;
                carToUpdate.GuaranteePeriod = carUpdated.GuaranteePeriod;
                carToUpdate.FirstRegistration = carUpdated.FirstRegistration;
                carToUpdate.RegistrationNumber = carUpdated.RegistrationNumber;
                carToUpdate.RegisterdInPoland = carUpdated.RegisterdInPoland;
                carToUpdate.FirstOwner = carUpdated.FirstOwner;
                carToUpdate.ASOServiced = carUpdated.ASOServiced;
                carToUpdate.AccidentFree = carUpdated.AccidentFree;
                carToUpdate.EngineCapacity = carUpdated.EngineCapacity;
                carToUpdate.Power = carUpdated.Power;
                carToUpdate.IsDamaged = carUpdated.IsDamaged;
                carToUpdate.IsRighthandDrive = carUpdated.IsRighthandDrive;
                carToUpdate.DPF = carUpdated.DPF;
                carToUpdate.CO2Emission = carUpdated.CO2Emission;
                carToUpdate.Generation = carUpdated.Generation;
                carToUpdate.Fuel = carUpdated.Fuel;
                carToUpdate.Gearbox = carUpdated.Gearbox;
                carToUpdate.ColorType = carUpdated.ColorType;
                carToUpdate.Body = carUpdated.Body;
                carToUpdate.Drive = carUpdated.Drive;
                #endregion
                var equ = DbContext.CarEquipments.Where(carEquipment => carEquipment.CarId == carToUpdate.Id);
                DbContext.CarEquipments.RemoveRange(equ);
                var carEquipments = new List<CarEquipment>();

                if (carEqu is not null)
                {
                    carEquipments.AddRange(carEqu.Select(equId => new CarEquipment {CarId = carToUpdate.Id, EquipmentId = equId}));
                }
                await DbContext.CarEquipments.AddRangeAsync(carEquipments);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
            }

        }
    }
}
