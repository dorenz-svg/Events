using Events.Abstractions;
using Events.Infrastructure;
using Events.Models;
using Events.Models.Entities;
using Events.Models.Request;
using Events.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.DataAccess.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly DBContext _context;

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IJwtGenerator jwtGenerator, DBContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerator = jwtGenerator;
            _context = context;
        }
        public async Task<AuthResponse> LogIn(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return null;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (result.Succeeded)
            {
                return new AuthResponse
                {
                    Token = _jwtGenerator.CreateToken(user)
                };
            }

            return null;
        }

        public async Task<AuthResponse> Registration(RegistrationRequest request)
        {
            if (await _context.Users.AsNoTracking().Where(x => x.Email == request.Email).AnyAsync())
            {
                return null;
            }

            if (await _context.Users.AsNoTracking().Where(x => x.UserName == request.UserName).AnyAsync())
            {
                return null;
            }

            var user = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.UserName
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                return new AuthResponse
                {
                    Token = _jwtGenerator.CreateToken(user)
                };
            }

            return null;
        }
    }
}
