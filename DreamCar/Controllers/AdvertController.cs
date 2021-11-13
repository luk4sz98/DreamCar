using AutoMapper;
using DreamCar.DAL.EF;
using DreamCar.Model.DataModels;
using DreamCar.Services.Interfaces;
using DreamCar.ViewModels.VM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
            UserManager<User> userManager,
            IAdvertService advertService
        ) : base(logger, mapper, userManager)
        {
            _equipmentService = equipmentService;
            _advertService = advertService;
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEdit(List<string> errors, Guid? advertId)
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

            if (advertId == null) return View();

            var userAdvert = await _advertService.GetAdvertToEditAsync((Guid)advertId);
            ViewBag.advertId = userAdvert.Advert.AdvertId;

            return View(userAdvert);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(CarVm car, CheckedEquVm equipments, ImageUploadedVm imagesUploaded, AdvertVm advert)
        {
            if (!ModelState.IsValid)
            {
                var errors = new List<string>();
                foreach (var error in ModelState["Images"].Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
                TempData["AdvertAdded"] = false;
                return RedirectToAction("AddNewAdvert", errors);
            }

            var addAdvertVm = new AddOrEditAdvertVm { Car = car, Equipments = equipments, ImagesUploaded = imagesUploaded, Advert = advert };

            var result = await _advertService.AddOrEditAdvertAsync(addAdvertVm);

            if (result.Item1)
            {
                TempData["AdvertAdded"] = true;
                return RedirectToAction("UserAdverts");
            }

            ModelState.AddModelError(string.Empty, result.Item2);
            TempData["AdvertAdded"] = false;
            return RedirectToAction("AddNewAdvert");
        }

        [HttpGet]
        [Route("UserAdverts/Active")]
        public async Task<IActionResult> ActiveAdverts()
        {
            var user = await UserManager.GetUserAsync(User);

            var activeAdverts = await _advertService.GetUserAdvertsAsync(
                                                    ad => ad.IsActive &&
                                                    ad.ClosedAt == null &&
                                                    ad.UserId == user.Id);
            return View(activeAdverts);
        }

        [HttpGet]
        [Route("UserAdverts/Pending")]
        public async Task<IActionResult> PendingAdverts()
        {
            var user = await UserManager.GetUserAsync(User);
            var pendingAdverts = await _advertService.GetUserAdvertsAsync(
                                                    ad => !ad.IsActive &&
                                                    ad.ClosedAt == null &&
                                                    ad.UserId == user.Id);

            return View("PendingAdverts", pendingAdverts);
        }

        [HttpGet]
        [Route("UserAdverts/Ended")]
        public async Task<IActionResult> EndedAdverts()
        {
            var user = await UserManager.GetUserAsync(User);
            var endedAdverts = await _advertService.GetUserAdvertsAsync(
                                                    ad =>
                                                    ad.ClosedAt != null &&
                                                    ad.UserId == user.Id);
            return View(endedAdverts);
        }

        [HttpPost]
        public async Task<IActionResult> EndAdvert(Guid advertId)
        {
            try
            {
                var result = await _advertService.EndAdvertAsync(advertId);
                if (result)
                    TempData["EndAdvertSuccess"] = "Ogłoszenie zakończone pomyślnie";
                else
                    TempData["EndAdvertError"] = "Coś poszło nie tak, spróbuj ponownie";
                return Json(new { redirectToUrl = Url.Action("ActiveAdverts", "Advert") });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                TempData["EndAdvertError"] = "Coś poszło nie tak, spróbuj ponownie";
                return Json(new { redirectToUrl = Url.Action("ActiveAdverts", "Advert") });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAdvert(Guid advertId)
        {
            try
            {
                var result = await _advertService.DeleteAdvertAsync(advertId);
                if (result)
                    TempData["DeleteAdvertSuccess"] = "Ogłoszenie usunięto pomyślnie";
                else
                    TempData["DeleteAdvertError"] = "Coś poszło nie tak, spróbuj ponownie";
                return Json(new { redirectToUrl = Url.Action("ActiveAdverts", "Advert") });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                TempData["DeleteAdvertError"] = "Coś poszło nie tak, spróbuj ponownie";
                return Json(new { redirectToUrl = Url.Action("ActiveAdverts", "Advert") });
            }
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetAdvert(Guid advertId)
        {
            try
            {
                var advert = await _advertService.GetAdvertAsync(advertId);
                return View("Advert", advert);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                TempData["GetAdvertError"] = "Coś poszło nie tak, spróbuj ponownie";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
