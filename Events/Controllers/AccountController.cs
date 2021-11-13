using Events.Abstractions;
using Events.Infrastructure;
using Events.Models;
using Events.Models.Entities;
using Events.Models.Request;
using Events.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> LoginAsync(LoginRequest request)
        {
            var result = await _accountRepository.LogIn(request);
            if (result is null)
                return BadRequest();
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("registration")]
        public async Task<ActionResult<AuthResponse>> RegistrationAsync(RegistrationRequest request)
        {
            var result = await _accountRepository.Registration(request);
            if (result is null)
                return BadRequest();
            return Ok(result);
        }
    }
}
