using AutoMapper;
using DreamCar.DAL.EF;
using DreamCar.Model.DataModels;
using DreamCar.Services.Interfaces;
using DreamCar.ViewModels.VM;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

        public async Task<Car> AddNewCarAsync(CarVm carVm, IEnumerable<int> carEqu)
        {
            try
            {
                var carEntity = Mapper.Map<Car>(carVm);
                
                await DbContext.Cars.AddAsync(carEntity);

                var carEquipments = new List<CarEquipment>();
                
                foreach (var equId in carEqu)
                {
                    carEquipments.Add(new CarEquipment { Car = carEntity, EquipmentId = equId });
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
    }
}
