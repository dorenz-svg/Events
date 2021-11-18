using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Model
{
    public class Visitors
    {
        public string UserName { get; set; }
        public IEnumerable<Date> Dates { get; set; }
    }

    public class Date
    {
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

        public bool IsInRange( DateTime dateStart)
        {
            return DateStart <= dateStart && (DateEnd is null || dateStart < DateEnd);
        }

        public bool IsInRange( DateTime? dateStart, DateTime? dateEnd)
        {
            return (DateStart <= dateStart && dateEnd <= DateEnd) && dateStart < dateEnd;
        }
    }
}
