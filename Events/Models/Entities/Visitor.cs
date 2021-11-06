using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Models.Entities
{
    public class Visitor
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public long EventId { get; set; }
        public Event Event { get; set; }
    }
}
