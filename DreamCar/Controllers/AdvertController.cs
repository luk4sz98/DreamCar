using AutoMapper;
using DreamCar.DAL.EF;
using DreamCar.Model.DataModels;
using DreamCar.Services.Interfaces;
using DreamCar.ViewModels.VM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DreamCar.Web.Controllers
{
    [Authorize(Roles = "Admin, Mod, User")]
    public class AdvertController : BaseController
    {
        private readonly IEquipmentService _equipmentService;
        private readonly ApplicationDbContext _applicationDbContext;
        public AdvertController(
            ILogger logger,
            IMapper mapper,
            IEquipmentService equipmentService,
            ApplicationDbContext applicationDbContext
        ) : base(logger, mapper)
        {
            _equipmentService = equipmentService;
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> AddNewAdvert(List<string> errors)
        {
            if (errors.Any())
            {
                ViewBag.ErrorList = errors;
            }

            ViewBag.YearsSelectList = new SelectList(
                Enumerable.Range(1900, (DateTime.Now.Year - 1900) + 1)).Reverse();

            ViewBag.EquipmentList = await _equipmentService.GetEquipmentsAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewAdvert( CheckedEquVm equipments, ImageVm images)
        {
            var carEquu = new List<CarEquipmentVm>();
            if (!ModelState.IsValid)
            {
                var errors = new List<string>();
                foreach (var error in ModelState["Image"].Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
                return  RedirectToAction("AddNewAdvert", errors);
            }
                
           
            
            foreach (var equId in equipments.Checked)
            {
                carEquu.Add(new CarEquipmentVm { CarId = 3, EquipmentId = equId });
            }
           
            //var advertvm = new AddAdvertVm { Car = car };
            return RedirectToAction("Index");
        }
    }
}
