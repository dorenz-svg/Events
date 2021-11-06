using Events.Infrastructure;
using Events.Models;
using Events.Models.Entities;
using Events.Models.Query;
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
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly DBContext _context;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IJwtGenerator jwtGenerator, DBContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerator = jwtGenerator;
            _context = context;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<object>> LoginAsync(LoginQuery query)
        {
            var user = await _userManager.FindByEmailAsync(query.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "Пользователь не найден" });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, query.Password, false);

            if (result.Succeeded)
            {
                return new
                {
                    Token = _jwtGenerator.CreateToken(user)
                };
            }

            return Unauthorized();
        }
        [AllowAnonymous]
        [HttpPost("registration")]
        public async Task<ActionResult<object>> RegistrationAsync([FromBody] RegistrationQuery query)
        {
            if (await _context.Users.Where(x => x.Email == query.Email).AnyAsync())
            {
                return BadRequest(new { message = "Email already exist" });
            }

            if (await _context.Users.Where(x => x.UserName == query.UserName).AnyAsync())
            {
                return BadRequest(new { message = "UserName already exist" });
            }

            var user = new ApplicationUser
            {
                Email = query.Email,
                UserName = query.UserName
            };

            var result = await _userManager.CreateAsync(user, query.Password);

            if (result.Succeeded)
            {
                return new
                {
                    Token = _jwtGenerator.CreateToken(user)
                };
            }

            return BadRequest(new { message = "Client creation failed" });
        }
    }
}
