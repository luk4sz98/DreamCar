﻿using AutoMapper;
using DreamCar.DAL.EF;
using DreamCar.Model.DataModels;
using DreamCar.Services.Interfaces;
using DreamCar.ViewModels.VM;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DreamCar.Services.Services
{
    public class EquipmentService : BaseService, IEquipmentService
    {
        public EquipmentService(
            ApplicationDbContext dbContext,
            ILogger logger,
            IMapper mapper,
            UserManager<User> userManager
            ) : base(dbContext, mapper, logger, userManager)
        {

        }
        public async Task<IEnumerable<EquipmentVm>> GetEquipmentsAsync(IEnumerable<int> equIds = null)
        {
            try
            {
                IEnumerable<Equipment> equipments = null;
                if (equIds is not null && equIds.Any()) {
                    equipments = await DbContext.Equipment
                        .Where(equ => equIds.Contains(equ.Id))
                        .ToListAsync();
                }
                else
                {
                   equipments = await DbContext.Equipment.ToListAsync();
                }
                               
                return Mapper.Map<IEnumerable<EquipmentVm>>(equipments);
             }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return new List<EquipmentVm>();
            }
           
        }
    }
}
