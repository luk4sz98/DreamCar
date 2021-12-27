using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using DreamCar.DAL.EF;
using DreamCar.Model.DataModels;
using DreamCar.Services.Interfaces;
using DreamCar.ViewModels.VM;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DreamCar.Services.Services
{
    public class MessageService : BaseService, IMessageService
    {
        public MessageService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager) 
            : base(dbContext, mapper, logger, userManager)
        {
        }

        public async Task<HttpStatusCode> CreateAdvertThread(AdvertThreadMessageVm advertThreadMessage)
        {
            try
            {
                var advertThreads =  DbContext.AdvertThreads
                    .Where(
                        at =>
                        at.CreatedBy == advertThreadMessage.SenderId &&
                        at.AdvertId == advertThreadMessage.AdvertId);
                if (advertThreads.Any())
                    return HttpStatusCode.BadRequest;

                var advertThread = Mapper.Map<AdvertThread>(advertThreadMessage);
                await DbContext.AdvertThreads.AddAsync(advertThread);
                await DbContext.SaveChangesAsync();

                var message = Mapper.Map<Message>(advertThreadMessage);
                message.AdvertThreadId = advertThread.Id;
                await DbContext.Messages.AddAsync(message);
                await DbContext.SaveChangesAsync();

                return HttpStatusCode.OK;

            }
            catch (Exception ex)
            {
                Logger.LogError(ex ,ex.Message);
                return HttpStatusCode.InternalServerError;
            }
        }
    }
}
