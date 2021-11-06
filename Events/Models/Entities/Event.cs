using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Models.Entities
{
    public class Event
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<Visitor> Visitors { get; set; }
        public ICollection<Date> Dates { get; set; }
    }
}
