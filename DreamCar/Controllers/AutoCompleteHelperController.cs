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
        public async Task<JsonResult> AutoComplete(string brand, string model, string prefix)
        {
            List<string> results;

            if (string.IsNullOrEmpty(brand))
                results = await _applicationDbContext.Cars
                                    .Where(car => car.Brand.StartsWith(prefix))
                                    .Select(car => car.Brand)
                                    .Distinct()
                                    .ToListAsync();
            else if (string.IsNullOrEmpty(model))
                results = await _applicationDbContext.Cars
                                    .Where(
                                        car => car.Brand.ToLower() == brand.ToLower() &&
                                        car.Model.StartsWith(prefix)
                                     )
                                    .Select(car => car.Model)
                                    .Distinct()
                                    .ToListAsync();
            else
                results = await _applicationDbContext.Cars
                                    .Where(
                                        car => car.Brand.ToLower() == brand.ToLower() &&
                                        car.Model.ToLower() == model.ToLower() &&
                                        car.Generation.StartsWith(prefix)
                                     )
                                    .Select(car => car.Generation)
                                    .Distinct()
                                    .ToListAsync();

            return Json(results);
        }
    }
}
