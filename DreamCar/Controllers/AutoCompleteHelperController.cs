using AutoMapper;
using DreamCar.DAL.EF;
using DreamCar.Model.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DreamCar.Web.Controllers
{
    [Authorize(Roles = "Admin, Mod, User")]
    public class AutoCompleteHelperController : BaseController
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public AutoCompleteHelperController(
            ILogger logger,
            IMapper mapper,
            ApplicationDbContext applicationDbContext,
            UserManager<User> userManager
        ) : base(logger, mapper, userManager)
        {
            _applicationDbContext = applicationDbContext;
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
