using AutoMapper;
using DreamCar.Model.DataModels;
using DreamCar.ViewModels.VM;
using DreamCar.Web.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DreamCar.Controllers
{
    public class HomeController : BaseController
    {

        public HomeController(
            ILogger logger,
            IMapper mapper,
            UserManager<User> userManager) : base(logger, mapper, userManager)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
