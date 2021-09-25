using AutoMapper;
using DreamCar.DAL.EF;
using DreamCar.Model.DataModels;
using DreamCar.Services.Interfaces;
using DreamCar.ViewModels.VM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DreamCar.Web.Controllers
{
    [Authorize(Roles = "Admin, Mod, User")]
    public class AdvertController : BaseController
    {
        private readonly IEquipmentService _equipmentService;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IAdvertService _advertService;

        private readonly SelectList _countries = new(new string[] {
            "Austria", "Belgia", "Białoruś", "Bułgaria",
            "Chorwacja", "Czechy", "Dania", "Estonia",
            "Finlandia", "Francja", "Grecja", "Hiszpania",
            "Holandia", "Irlandia", "Islandia", "Kanada",
            "Liechtenstein", "Litwa", "Luksemburg", "Łotwa",
            "Monako", "Niemcy", "Norwegia", "Polska",
            "Rosja", "Rumunia", "Słowacja", "Słowenia",
            "Stany Zjednoczone", "Szwajcaria", "Szwecja", "Turcja",
            "Ukraina", "Węgry", "Wielka Brytania", "Włochy",
            "Inny" 
        });

        private readonly SelectList _months = new(CultureInfo.GetCultureInfo("pl-Pl").DateTimeFormat.MonthNames);

        public AdvertController(
            ILogger logger,
            IMapper mapper,
            IEquipmentService equipmentService,
            ApplicationDbContext applicationDbContext,
            UserManager<User> userManager,
            IAdvertService advertService
        ) : base(logger, mapper, userManager)
        {
            _equipmentService = equipmentService;
            _applicationDbContext = applicationDbContext;
            _advertService = advertService;
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
            ViewBag.SeatsNumber = new SelectList(Enumerable.Range(1, 9));
            ViewBag.Countries = _countries;
            ViewBag.Months = _months;
            ViewBag.Days = new SelectList(Enumerable.Range(1, 31));
            ViewBag.Years = new SelectList(Enumerable.Range(DateTime.Now.Year, (DateTime.Now.Year + 10) - DateTime.Now.Year));
            ViewBag.User = await UserManager.GetUserAsync(User);
                       
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewAdvert(CarVm car, CheckedEquVm equipments, ImageUploadedVm imagesUploaded, AdvertVm advert)
        {
            if (!ModelState.IsValid)
            {
                var errors = new List<string>();
                foreach (var error in ModelState["Images"].Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
                TempData["AdvertAdded"] = false;
                return  RedirectToAction("AddNewAdvert", errors);
            }
            
            var addAdvertVm = new AddAdvertVm { Car = car, Equipments = equipments, ImagesUploaded = imagesUploaded, Advert = advert };

            var result = await _advertService.AddNewAdvertAsync(addAdvertVm);

            if(result.Item1)
            {
                TempData["AdvertAdded"] = true;
                return RedirectToAction("Index", "Home"); //temporary
            }

            ModelState.AddModelError(string.Empty, result.Item2);
            TempData["AdvertAdded"] = false;
            return RedirectToAction("AddNewAdvert");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AutoCompleteBrand(string prefix)
        {
            var brands = await _applicationDbContext.Cars
                                    .Where(car => car.Brand.StartsWith(prefix))
                                    .Select(car => car.Brand)
                                    .Distinct()
                                    .ToListAsync();
            return Json(brands);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AutoCompleteModel(string brand, string prefixModel)
        {
            var models = await _applicationDbContext.Cars
                                    .Where(
                                        car => car.Brand.ToLower() == brand.ToLower() &&
                                        car.Model.StartsWith(prefixModel)
                                     )
                                    .Select(car => car.Model)
                                    .Distinct()
                                    .ToListAsync();
            return Json(models);
        }
    }
}
