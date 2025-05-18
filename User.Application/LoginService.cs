using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Exceptions;
using User.Application;
using User.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using User.Domain.Models;

namespace User.Application
{
    public class LoginService : ILoginService
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly UsersDataContext _context;
        private readonly IPasswordHasher<UserModel> _passwordHasher;
        public LoginService(IJwtTokenService jwtTokenService, UsersDataContext context, IPasswordHasher<UserModel> passwordHasher)
        {
            _jwtTokenService = jwtTokenService;
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> Login(string login, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName== login);
            if (user != null)
            {

                var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, password);

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

    public interface ILoginService
    {
        Task<string> Login(string username, string password);
    }
}
