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
        public Task<string> CreateEvent(string idUser,string name);
        public Task AddVisitorAndDates(long idEvent,string idUser,  Dictionary<DateTime, DateTime?> dates);
        public Task<EventsResponse> GetListEvents(string idUser);
        public Task<(DateTime?,DateTime?)> KnowTimeEvent(string idUser,long idEvent);
    }
}
