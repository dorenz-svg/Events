using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Models.Entities
{
    public class Date
    {
        public long Id { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime? DateEnd { get; set; }
        public long EventId { get; set; }
        public Event Event { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
