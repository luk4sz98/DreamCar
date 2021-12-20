using AutoMapper;
using DreamCar.DAL.EF;
using DreamCar.Model.DataModels;
using DreamCar.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace DreamCar.Services.Services
{
    public class SessionService : BaseService, ISessionService
    {
        private readonly IHttpContextAccessor _httpContext;

        public SessionService(
            ApplicationDbContext dbContext,
            IMapper mapper,
            ILogger logger,
            UserManager<User> userManager,
            IHttpContextAccessor httpContext) : base(dbContext, mapper, logger, userManager)
        {
            _httpContext = httpContext;
        }

        public string Get(string key)
        {
            return _httpContext.HttpContext != null 
                ? _httpContext.HttpContext.Session.GetString(key) 
                : string.Empty;
        }

        public void Remove(string key)
        {
            _httpContext.HttpContext?.Session.Remove(key);
        }

        public void Set(string key, string value)
        {
            _httpContext.HttpContext?.Session.SetString(key, value);
        }
    }
}
