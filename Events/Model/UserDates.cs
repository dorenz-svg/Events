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

    public class Date
    {
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

        public bool IsInRange(Date date, DateTime dateStart)
        {
            return date.DateStart <= dateStart && (date.DateEnd is null || dateStart < date.DateEnd);
        }

        public bool IsInRange(Date date, DateTime dateStart, DateTime dateEnd)
        {
            return (date.DateStart <= beginDate && dateEnd >= date.DateEnd) && beginDate < dateEnd;
        }
    }
}
