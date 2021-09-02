using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DreamCar.Web.Controllers
{
    [Authorize(Roles = "Admin, Client, User")]
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
