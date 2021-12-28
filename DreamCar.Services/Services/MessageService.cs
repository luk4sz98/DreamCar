using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
                        at.CreatedById == advertThreadMessage.SenderId &&
                        at.AdvertId == advertThreadMessage.AdvertId &&
                        at.Subject == advertThreadMessage.Subject)
                    .AsEnumerable();
                advertThreads = advertThreads.Where(at => (DateTime.Now - at.CreatedAt).TotalDays < 14);
                if (advertThreads.Any())
                    return HttpStatusCode.BadRequest;

                var advertThread = Mapper.Map<AdvertThread>(advertThreadMessage);

                var sender = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == advertThreadMessage.SenderId);
                if (sender is null) return HttpStatusCode.BadRequest;

                advertThread.CreatedBy = string.Concat(sender.FirstName, " ", sender.LastName);
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

        public async Task<IEnumerable<AdvertThreadVm>> GetAdvertThreads(Expression<Func<AdvertThread, bool>> filterExpressions)
        {
            try
            {
                var advertThreads = await DbContext.AdvertThreads
                    .Where(filterExpressions)
                    .ToListAsync();
                return Mapper.Map<IEnumerable<AdvertThreadVm>>(advertThreads);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return Enumerable.Empty<AdvertThreadVm>();
            }
        }
    }
}
