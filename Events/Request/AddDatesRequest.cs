using System;
using System.Collections.Generic;

namespace Events.Models.Request
{
    public class AddDatesRequest
    {
        public IEnumerable<Date> Dates { get; set; }
    }

    public class Date
    {
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }
}
