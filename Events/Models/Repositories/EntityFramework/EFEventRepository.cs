using Events.Infrastructure;
using Events.Models.Entities;
using Events.Models.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Models.Repositories
{
    public class EFEventRepository : IEventRepository
    {
        private readonly DBContext context;
        private readonly IAlgorithm algorithm;
        public EFEventRepository(DBContext ctx,IAlgorithm alg) 
        {
            algorithm = alg;
            context = ctx; 
        }

        public async Task AddVisitorAndDates(long idEvent, string idUser, Dictionary<DateTime, DateTime?> dates)
        {
            foreach (var date in dates)
            {
                if (date.Key > DateTime.UtcNow && date.Value is null || date.Value > DateTime.UtcNow)
                {
                    var tempDate = new Date() { Id = 0, DateBegin = date.Key, DateEnd = date.Value, EventId = idEvent, UserId = idUser };
                    context.Dates.Add(tempDate);
                }
            }
            if (await context.Visitors.Where(x => x.EventId == idEvent && x.UserId == idUser).AnyAsync())
            {
                context.Visitors.Add(new Visitor { Id = 0, EventId = idEvent, UserId = idUser });
            }
            await context.SaveChangesAsync();
        }
        public async Task<string> CreateEvent(string idUser, string name)
        {
            var tempEvent = new Event() { Id = 0, Name = name, UserId = idUser };
            context.Events.Add(tempEvent);
            await context.SaveChangesAsync();
            return tempEvent.Id.ToString();
        }

        public async Task<IEnumerable<string>> GetListEvents(string idUser)
            => await context.Visitors.Include(x => x.Event).Where(x => x.UserId == idUser).Select(x => x.Event.Name).ToListAsync();

        public async Task<(DateTime?, DateTime?)> KnowTimeEvent(string idUser, long idEvent)
        {
            if (!await context.Events.Where(x => x.Id == idEvent && x.UserId == idUser).AnyAsync())
            {
                var tempDate = await (from x in context.Users.Include(x => x.Dates)
                               select x.Dates.Where(c=>c.UserId==x.Id && c.EventId==idEvent)
                               .ToDictionary(x=>x.DateBegin,y=>y.DateEnd)).ToListAsync();
                return algorithm.GetDate(tempDate);
            }
            return (null, null) ;
        }
    }
}
