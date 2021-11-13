using System.Collections.Generic;

namespace Events.Models.Response
{
    public class EventsResponse
    {
        public IEnumerable<EventResponse> CreatedEvents { get; set; }
        public IEnumerable<EventResponse> AcceptedEvents { get; set; }
    }
    public class EventResponse {
        public string Name { get; set; }
        public long IdEvent { get; set; }
    }

}
