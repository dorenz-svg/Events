using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Models.Query
{
    public class AddDatesQuery
    {
        [Required]
        public long? IdEvent { get; set; }
        public Dictionary<DateTime,DateTime?> Dates { get; set; }
    }
}
