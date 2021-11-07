using AutoMapper;
using DreamCar.DAL.EF;
using DreamCar.Model.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace DreamCar.Services.Services
{
    public abstract class BaseService
    {
        protected readonly ApplicationDbContext DbContext;
        protected readonly ILogger Logger;
        protected readonly IMapper Mapper;

        protected readonly UserManager<User> UserManager;
        protected BaseService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager)
        {
            DbContext = dbContext;
            Mapper = mapper;
            Logger = logger;
            UserManager = userManager;
        }
    }
}
