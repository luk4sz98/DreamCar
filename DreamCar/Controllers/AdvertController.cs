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
                return RedirectToAction("AddNewAdvert", errors);
            }

            var addAdvertVm = new AddAdvertVm { Car = car, Equipments = equipments, ImagesUploaded = imagesUploaded, Advert = advert };

            var result = await _advertService.AddNewAdvertAsync(addAdvertVm);

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
        public async Task<IActionResult> UserAdverts(int? page, int? pageSize, string filterValue = null, string advertType = null)
        {
            var user = await UserManager.GetUserAsync(User);
            bool isAjaxRequest = HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest";
            if (isAjaxRequest && !string.IsNullOrEmpty(filterValue))
            {
                IEnumerable<UserAdvertVm> filterAdverts = null;
                filterAdverts = advertType switch
                {
                    "Active" => _advertService.GetUserAdvertsAsQueryable(
                                               ad =>
                                               ad.IsActive &&
                                               ad.ClosedAt == null &&
                                               ad.Title.ToLower()
                                                   .StartsWith(filterValue.ToLower()) &&
                                               ad.UserId == user.Id),
                    "Pending" => _advertService.GetUserAdvertsAsQueryable(
                                                ad =>
                                                !ad.IsActive &&
                                                ad.ClosedAt == null &&
                                                ad.Title.ToLower()
                                                  .StartsWith(filterValue.ToLower()) &&
                                                ad.UserId == user.Id),
                    _ => _advertService.GetUserAdvertsAsQueryable(
                                                ad =>
                                                ad.ClosedAt != null &&
                                                ad.Title.ToLower()
                                                  .StartsWith(filterValue.ToLower()) &&
                                                ad.UserId == user.Id),
                };
                return PartialView("_AdvertTableDataPartial", filterAdverts);
            }

            ViewBag.ActiveAdverts = await _advertService.GetUserAdvertsAsync(
                                                ad => ad.IsActive &&
                                                ad.ClosedAt == null &&
                                                ad.UserId == user.Id
                                            );

            ViewBag.PendingAdverts = await _advertService.GetUserAdvertsAsync(
                                                ad => !ad.IsActive &&
                                                ad.ClosedAt == null &&
                                                ad.UserId == user.Id
                                            );

            ViewBag.EndedAdverts = await _advertService.GetUserAdvertsAsync(
                                            ad =>
                                            ad.ClosedAt != null &&
                                            ad.UserId == user.Id
                                        );
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetUserSpecificAdverts(string advertType)
        {
            try
            {
                var user = await UserManager.GetUserAsync(User);

                switch (advertType)
                {
                    case "Pending":
                        var userPendingAdverts = await _advertService.GetUserAdvertsAsync(
                                                                        ad => !ad.IsActive &&
                                                                        ad.ClosedAt == null &&
                                                                        ad.UserId == user.Id
                                                                    );
                        return PartialView("_AdvertTableDataPartial", userPendingAdverts);
                    case "Ended":
                        var userEndedAdverts = await _advertService.GetUserAdvertsAsync(
                                                        ad =>
                                                        ad.ClosedAt != null &&
                                                        ad.UserId == user.Id
                                                    );
                        return PartialView("_AdvertTableDataPartial", userEndedAdverts);
                    default:
                        var userActiveAdverts = await _advertService.GetUserAdvertsAsync(
                                                                        ad => ad.IsActive &&
                                                                        ad.ClosedAt == null &&
                                                                        ad.UserId == user.Id
                                                                    );
                        return PartialView("_AdvertTableDataPartial", userActiveAdverts);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return PartialView("_AdvertTableDataPartial", null);
            }
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
                return Json(new { redirectToUrl = Url.Action("UserAdverts", "Advert") });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                TempData["EndAdvertError"] = "Coś poszło nie tak, spróbuj ponownie";
                return Json(new { redirectToUrl = Url.Action("UserAdverts", "Advert") });
            }
        }
    }
}
