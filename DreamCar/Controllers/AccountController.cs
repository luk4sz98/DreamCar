using AutoMapper;
using Microsoft.Extensions.Logging;
using DreamCar.Model.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DreamCar.Web.Controllers
{
    [Authorize(Roles = "Admin, Client, User")]
    public class AccountController : BaseController
    {
        private readonly UserManager<User> _userManager;

        public AccountController(
                UserManager<User> userManager,
                ILogger logger,
                IMapper mapper
            ) : base(logger, mapper)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
