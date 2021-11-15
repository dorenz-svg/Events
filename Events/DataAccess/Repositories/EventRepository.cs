using Events.Infrastructure;
using Events.Models.Entities;
using Events.Models.Request;
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
    public class EventRepository : IEventRepository
    {
        private readonly DBContext _context;
        private readonly IAlgorithm _algorithm;
        public EventRepository(DBContext context, IAlgorithm algorithm)
        {
            _algorithm = algorithm;
            _context = context;
        }
        /// <summary>
        /// фукнция сохраняет диапозоны дат (будующие даты)
        /// и добавляет пользователя в список поситителей
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="userId"></param>
        /// <param name="dates">словарь диапозонов дат</param>
        /// <returns></returns>
        public async Task AddVisitorAndDates(long eventId, string userId, IEnumerable<Request.Date> dates)
        {
            foreach (var date in dates)
            {
                bool isFutureDate = date.DateStart > DateTime.UtcNow && date.DateEnd is null || date.DateEnd > DateTime.UtcNow;
                if (isFutureDate)
                {
                    var tempDate = new Entities.Date() { Id = 0, DateBegin = date.DateStart, DateEnd = date.DateEnd, EventId = eventId, UserId = userId };
                    _context.Dates.Add(tempDate);
                }
            }

            bool isVisitorAdded = !(await _context.Visitors.AsNoTracking().Where(x => x.EventId == eventId && x.UserId == userId).AnyAsync());
            if (isVisitorAdded)
            {
                _context.Visitors.Add(new Visitor { Id = 0, EventId = eventId, UserId = userId });
            }

            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// функция создает событие 
        /// и добавляет пользователя в список участников этого события
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="nameEvent"></param>
        /// <returns>id события</returns>
        public async Task<long> CreateEvent(string userId, string nameEvent)
        {
            var Event = new Event() { Id = 0, Name = nameEvent, UserId = userId };
            _context.Events.Add(Event);
            _context.Visitors.Add(new Visitor { Id = 0, Event = Event, UserId = userId });
            await _context.SaveChangesAsync();
            return Event.Id;
        }
        /// <summary>
        /// функция возвращает список принятых событий
        /// и список созданных событий
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>возвращает список названий и id событий</returns>
        public async Task<EventsResponse> GetListEvents(string userId)
        {
            return new EventsResponse
            {
                CreatedEvents = await _context.Events.AsNoTracking()
                                             .Where(x => x.UserId == userId)
                                             .Select(x => new EventResponse { Name = x.Name, IdEvent = x.Id })
                                             .ToListAsync(),
                AcceptedEvents = await _context.Visitors.AsNoTracking().Include(x => x.Event)
                                             .Where(x => x.UserId == userId)
                                             .Select(x => new EventResponse { Name = x.Event.Name, IdEvent = x.Event.Id })
                                             .ToListAsync()
            };
        }
        /// <summary>
        /// функция возвращает пересечение желаемых дат
        /// для пользователя создавшего событие
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task<(DateTime? DateBegin, DateTime? DateEnd)> GetEventDate(string userId, long eventId)
        {
            bool isOwnerEvent = await _context.Events.AsNoTracking().Where(x => x.Id == eventId && x.UserId == userId).AnyAsync();
            if (isOwnerEvent)
            {
                var tempDate = await (from x in _context.Users.AsNoTracking().Include(x => x.Dates)
                                      select x.Dates.Where(c => c.UserId == x.Id && c.EventId == eventId)
                                                    .Select(x => new { DateBegin = x.DateBegin, DateEnd = x.DateEnd }))
                                      .ToListAsync();

                var temp = tempDate.Select(x => x.ToDictionary(k => k.DateBegin, y => y.DateEnd)).ToList();

                return _algorithm.FindConvenientDate(null);
            }
            return (null, null);
        }
    }
}
