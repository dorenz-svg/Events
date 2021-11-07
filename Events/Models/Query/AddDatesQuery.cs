using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Models.Query
{
    public class AddDatesQuery
    {
        public long IdEvent { get; set; }
        public Dictionary<DateTime,DateTime?> Dates { get; set; }
    }
}
