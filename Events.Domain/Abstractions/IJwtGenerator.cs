using Events.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Infrastructure
{
    public interface IJwtGenerator
    {
        string CreateToken(ApplicationUser user);
    }
}
