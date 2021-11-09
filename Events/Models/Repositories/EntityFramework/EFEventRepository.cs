using Events.Infrastructure;
using Events.Models.Entities;
using Events.Models.Repositories.Abstract;
using Events.Models.Response;
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
        public EFEventRepository(DBContext ctx, IAlgorithm alg)
        {
            algorithm = alg;
            context = ctx;
        }
        /// <summary>
        /// фукнция сохраняет диапозоны дат (будующие даты)
        /// и добавляет пользователя в список поситителей
        /// </summary>
        /// <param name="idEvent"></param>
        /// <param name="idUser"></param>
        /// <param name="dates">словарь диапозонов дат</param>
        /// <returns></returns>
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
            if (!(await context.Visitors.Where(x => x.EventId == idEvent && x.UserId == idUser).AnyAsync()))
            {
                context.Visitors.Add(new Visitor { Id = 0, EventId = idEvent, UserId = idUser });
            }
            await context.SaveChangesAsync();
        }
        /// <summary>
        /// функция создает событие 
        /// и добавляет пользователя в список участников этого события
        /// </summary>
        /// <param name="idUser"></param>
        /// <param name="name"></param>
        /// <returns>id события</returns>
        public async Task<string> CreateEvent(string idUser, string name)
        {
            var tempEvent = new Event() { Id = 0, Name = name, UserId = idUser };
            context.Events.Add(tempEvent);
            context.Visitors.Add(new Visitor { Id = 0, Event = tempEvent, UserId = idUser });
            await context.SaveChangesAsync();
            return tempEvent.Id.ToString();
        }
        /// <summary>
        /// функция возвращает список принятых событий
        /// и список созданных событий
        /// </summary>
        /// <param name="idUser"></param>
        /// <returns>возвращает список названий и id событий</returns>
        public async Task<EventsResponse> GetListEvents(string idUser)
        {
            return new EventsResponse
            {
                CreatedEvents = await context.Events
                                             .Where(x => x.UserId == idUser)
                                             .Select(x => new EventRes { Name = x.Name, IdEvent = x.Id })
                                             .ToListAsync(),
                AcceptedEvents = await context.Visitors.Include(x => x.Event)
                                             .Where(x => x.UserId == idUser)
                                             .Select(x => new EventRes { Name = x.Event.Name, IdEvent = x.Event.Id })
                                             .ToListAsync()
            };
        }
        /// <summary>
        /// функция возвращает пересечение желаемых дат
        /// для пользователя создавшего событие
        /// </summary>
        /// <param name="idUser"></param>
        /// <param name="idEvent"></param>
        /// <returns></returns>
        public async Task<(DateTime?, DateTime?)> KnowTimeEvent(string idUser, long idEvent)
        {
            if (await context.Events.Where(x => x.Id == idEvent && x.UserId == idUser).AnyAsync())
            {
                var tempDate = await (from x in context.Users.Include(x => x.Dates)
                                      select x.Dates.Where(c => c.UserId == x.Id && c.EventId == idEvent)
                                      .Select(x => new { DateBegin = x.DateBegin, DateEnd = x.DateEnd }))
                               .ToListAsync();
                var temp = tempDate.Select(x => x.ToDictionary(k => k.DateBegin, y => y.DateEnd)).ToList();
                return algorithm.GetDate(temp);
            }
            return (null, null);
        }
    }
}
