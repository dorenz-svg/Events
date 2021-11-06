﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Models.Query
{
    public class RegistrationQuery:LoginQuery
    {
        [BindRequired]
        public string UserName { get; set; }
    }
}
