using AutoMapper;
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
using System.Net;
using System.Threading.Tasks;

namespace DreamCar.Web.Controllers
{
    public class AdvertController : BaseController
    {
        private readonly IEquipmentService _equipmentService;
        private readonly IAdvertService _advertService;
        private readonly ICookiesService _cookiesService;
        private readonly SignInManager<User> _signInManager;

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
            IAdvertService advertService,
            ICookiesService cookiesService,
            SignInManager<User> signInManager
        ) : base(logger, mapper, userManager)
        {
            _equipmentService = equipmentService;
            _advertService = advertService;
            _cookiesService = cookiesService;
            _signInManager = signInManager;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Mod, User")]
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
        [Authorize(Roles = "Admin, Mod, User")]
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
                return RedirectToAction("ActiveAdverts");
            }

            ModelState.AddModelError(string.Empty, result.Item2);
            TempData["AdvertAdded"] = false;
            return RedirectToAction("AddNewAdvert");
        }

        [HttpGet]
        [Route("UserAdverts/Active")]
        [Authorize(Roles = "Admin, Mod, User")]
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
        [Authorize(Roles = "Admin, Mod, User")]
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
        [Authorize(Roles = "Admin, Mod, User")]
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
        [Authorize(Roles = "Admin, Mod, User")]
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
        [Authorize(Roles = "Admin, Mod, User")]
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
        [Route("Advert")]
        public async Task<IActionResult> GetAdvert(Guid advertId)
        {
            try
            {
                var advert = await _advertService.GetAdvertAsync(advertId);
                ViewBag.OthersAdvert = await _advertService.GetUserAdvertsAsync(
                    ad =>
                    ad.Id != advertId &&
                    !ad.IsActive &&
                    ad.ClosedAt == null &&
                    ad.UserId == advert.User.Id);

                var user = await UserManager.GetUserAsync(User);

                if (_signInManager.IsSignedIn(User))
                {
                    ViewBag.IsFollowed = await _advertService.IsFollowedAdvert(advertId, user.Id);
                }
                else
                {
                    var followedAdverts = _cookiesService.Get("follow");
                    if (string.IsNullOrEmpty(followedAdverts))
                        ViewBag.IsFollowed = false;
                    else if (followedAdverts.Contains(advertId.ToString()))
                        ViewBag.IsFollowed = true;
                    else
                        ViewBag.IsFollowed = false;
                }
                return View("Advert", advert);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                TempData["GetAdvertError"] = "Coś poszło nie tak, spróbuj ponownie";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<HttpStatusCode> FollowAdvert(Guid advertId)
        {
            try
            {
                if (_signInManager.IsSignedIn(User))
                {
                    var user = await UserManager.GetUserAsync(User);
                    await _advertService.FollowAdvert(advertId, user.Id);
                    return HttpStatusCode.OK;
                }

                var followAdverts = _cookiesService.Get("follow");
                if (string.IsNullOrEmpty(followAdverts))
                {
                    //by default, expire time is set to 5 days from today
                    _cookiesService.Set("follow", advertId.ToString());
                    return HttpStatusCode.OK;
                }

                followAdverts += ";" + advertId.ToString();
                _cookiesService.Set("follow", followAdverts);
                
                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return HttpStatusCode.InternalServerError;
            }
        }

        [HttpPost]
        public async Task<HttpStatusCode> UnfollowAdvert(Guid advertId)
        {
            try
            {
                if (_signInManager.IsSignedIn(User))
                {
                    var user = await UserManager.GetUserAsync(User);
                    await _advertService.UnfollowAdvert(advertId, user.Id);
                    return HttpStatusCode.OK;
                }

                var followAdverts = _cookiesService.Get("follow");
                followAdverts = followAdverts.Replace(advertId.ToString(), string.Empty);
                _cookiesService.Set("follow", followAdverts);

                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return HttpStatusCode.InternalServerError;
            }
        }

        [HttpGet]
        [Route("Adverts/Follow")]
        public async Task<IActionResult> GetFollowAdverts()
        {
            try
            {
                IEnumerable<UserAdvertVm> followAdverts;
                if (_signInManager.IsSignedIn(User))
                {
                    var user = await UserManager.GetUserAsync(User);
                    followAdverts = await _advertService.GetFollowAdvertsAsync(null, user.Id);
                    return View("FollowAdverts", followAdverts);
                }

                var followAdvertsId = _cookiesService.Get("follow") is null 
                    ?  Enumerable.Empty<Guid>() :
                    _cookiesService.Get("follow")
                        .Split(';')
                        .Where(id => !string.IsNullOrEmpty(id) &&
                                        !string.Equals(id, ";"))
                        .Select(id => Guid.Parse(id));


                followAdverts = await _advertService.GetFollowAdvertsAsync(followAdvertsId, null);              

                return View("FollowAdverts", followAdverts);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                TempData["GetAdvertError"] = "Coś poszło nie tak, spróbuj ponownie";
                return RedirectToAction("Index", "Home");
            }
        }

        [Route("Adverts")]
        public IActionResult GetAdverts(
            int? page,
            FilterVm filter = null,
            string sortOrder = null)
        {
            IQueryable<Advert> adverts = FilterCollection(_advertService.GetAdverts(), filter);

            if (filter is not null)
                TempData["filter"] = filter;

            adverts = sortOrder switch
            {
                "price_desc" => adverts.OrderByDescending(ad => ad.Price),
                "price" => adverts.OrderBy(ad => ad.Price),
                "mileage_desc" => adverts.OrderByDescending(ad => ad.Car.Mileage),
                "mileage" => adverts.OrderBy(ad => ad.Car.Mileage),
                "power_desc" => adverts.OrderByDescending(ad => ad.Car.Power),
                "power" => adverts.OrderBy(ad => ad.Car.Power),
                "productionYear_desc" => adverts.OrderByDescending(ad => ad.Car.ProductionYear),
                "productionYear" => adverts.OrderBy(ad => ad.Car.ProductionYear),
                _ => adverts.OrderBy(ad => ad.CreatedAt),
            };

            var pageSize = 20;
            page ??= 1;
            ViewBag.page = page;

            int start = (int)(page - 1) * pageSize;
            
            //Aktualny numer strony
            ViewBag.pageCurrent = page;
            int totalPages = adverts.Count();

            //Ilość wszystkich stron do wyświetlenia
            ViewBag.totalNumsize = (totalPages / (float)pageSize);
            ViewBag.numSize = (int)Math.Ceiling(ViewBag.totalNumsize);

            adverts = adverts.Skip(start).Take(pageSize);

            var mappedList = Mapper.Map<IEnumerable<UserAdvertVm>>(adverts);

            return View("Adverts", mappedList);
        }

        private static IQueryable<Advert> FilterCollection(IQueryable<Advert> collection, FilterVm filter)
        {
            if (!string.IsNullOrEmpty(filter.Brand))
                collection = collection.Where(ad => ad.Car.Brand.ToLower() == filter.Brand.ToLower());
            if (!string.IsNullOrEmpty(filter.Model))
                collection = collection.Where(ad => ad.Car.Model.ToLower() == filter.Model.ToLower());
            if (!string.IsNullOrEmpty(filter.Generation))
                collection = collection.Where(ad => ad.Car.Generation.ToLower() == filter.Generation.ToLower());
            if (filter.Fuel.HasValue)
                collection = collection.Where(ad => ad.Car.Fuel == filter.Fuel);
            if (filter.Body.HasValue)
                collection = collection.Where(ad => ad.Car.Body == filter.Body);
            return collection;
        }
    }
}
