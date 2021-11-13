using Events.Models.Request;
using Events.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Abstractions
{
    public interface IAccountRepository
    {
        public Task<AuthResponse> Registration(RegistrationRequest request);
        public Task<AuthResponse> LogIn(LoginRequest request);
    }
}
