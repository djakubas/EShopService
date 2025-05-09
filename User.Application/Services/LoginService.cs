using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Exceptions;
using User.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using User.Domain.Models;
using User.Application.Interfaces;

namespace User.Application.Services
{
    public class LoginService : ILoginService
    {
        protected IJwtTokenService _jwtTokenService;
        protected readonly UsersDataContext _context;
        public LoginService(IJwtTokenService jwtTokenService, UsersDataContext context)
        {
            _jwtTokenService = jwtTokenService;
            _context = context;
        }

        public async Task<string> Login(string login, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == login);
            if (user != null)
            {

                var hasher = new PasswordHasher<UserModel>();
                var result = hasher.VerifyHashedPassword(user, user.PasswordHash!, password);

                if (result == PasswordVerificationResult.Success)
                {
                    var roles = new List<string>();
                    roles.Add(user.Role);

                    var token = _jwtTokenService.GenerateToken(111, roles);
                    return token;
                }
                throw new InvalidCredentialsException();
            }
            else
            {
                throw new InvalidCredentialsException();
            }
        }
    }


}
