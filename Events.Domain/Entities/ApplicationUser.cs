using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Models.Entities
{
    public class ApplicationUser:IdentityUser
    {
        public IEnumerable<Visitor> Visitors { get; set; }
        public IEnumerable<Date> Dates { get; set; }
        public IEnumerable<Event> Events { get; set; }
    }
}
