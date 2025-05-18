using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Repositories;
using User.Domain.Models;
using User.Domain.Exceptions;

namespace User.Application
{
    public class RegisterService : IRegisterService
    {
        private readonly UsersDataContext _context;
        private readonly IPasswordHasher<UserModel> _passwordHasher;
        private readonly IPasswordValidator<UserModel> _passwordValidator;
        public RegisterService(UsersDataContext context, IPasswordHasher<UserModel> passwordHasher, IPasswordValidator<UserModel> passwordValidator)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _passwordValidator = passwordValidator;
        }
        public async Task RegisterAsync(string username, string password)
        {
            
            //todo: Unique username validation
            var user = new UserModel()
            {
                UserName = username,
                Role = "Client"
            };
            var pwdValidationResult = await _passwordValidator.ValidateAsync(null!, user, password);
            if (!pwdValidationResult.Succeeded)
            {
                //todo: I could add specific errors to the exception
                throw new PasswordValidationException();
            }
            var hashedPassword = _passwordHasher.HashPassword(user, password);
            user.PasswordHash = hashedPassword;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }

    public interface IRegisterService
    {
        public Task RegisterAsync(string username, string password);
    }
}
