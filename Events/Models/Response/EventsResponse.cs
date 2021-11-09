using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Models.Response
{
    public class EventsResponse
    {
        public IEnumerable<EventRes> CreatedEvents { get; set; }
        public IEnumerable<EventRes> AcceptedEvents { get; set; }
    }
    public class EventRes {
        public string Name { get; set; }
        public long IdEvent { get; set; }
    }

}
