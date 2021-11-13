using Events.Models.Request;
using Events.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Models.Repositories.Abstract
{
    public interface IEventRepository
    {
        public Task<long> CreateEvent(string idUser,string name);
        public Task AddVisitorAndDates(long idEvent,string idUser,  IEnumerable<Date> dates);
        public Task<EventsResponse> GetListEvents(string idUser);
        public Task<(DateTime? DateBegin, DateTime? DateEnd)> GetEventDate(string idUser,long idEvent);
    }
}
