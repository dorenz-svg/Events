using Events.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Model
{
    public class UserDates
    {
        public string UserName { get; set; }
        public IEnumerable<Date> Dates { get; set; }
    }
}
