using AutoMapper;
using DreamCar.Model.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DreamCar.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly ILogger Logger;
        protected readonly IMapper Mapper;
        protected readonly UserManager<User> UserManager;
        protected BaseController(ILogger logger, IMapper mapper, UserManager<User> userManager)
        {
            Logger = logger;
            Mapper = mapper;
            UserManager = userManager;
        }
    }
}