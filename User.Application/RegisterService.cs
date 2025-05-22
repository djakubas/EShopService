using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Repositories;
using User.Domain.Models;
using User.Domain.Exceptions;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace User.Application
{
    public class RegisterService : IRegisterService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IPasswordHasher<UserModel> _passwordHasher;
        private readonly IPasswordValidateService _passwordValidator;
        private readonly IUniqueUserValidateService _uniqueUserValidateService;
        public RegisterService(
            IUsersRepository usersRepository, 
            IPasswordHasher<UserModel> passwordHasher, 
            IPasswordValidateService passwordValidator,
            IUniqueUserValidateService uniqueUserValidateService)
        {
            _usersRepository = usersRepository;
            _passwordHasher = passwordHasher;
            _passwordValidator = passwordValidator;
            _uniqueUserValidateService = uniqueUserValidateService;
        }
        public async Task RegisterAsync(string username, string password)
        {
            var uniqueUserResult = await _uniqueUserValidateService.CheckUniqueUsername(username);
            if (!uniqueUserResult)
            {
                throw new UserAlreadyExistsException();
            }
            var user = new UserModel()
            {
                UserName = username,
                Role = "Client"
            };
            var pwdValidationErrors = _passwordValidator.ValidatePassword(password);
            if (pwdValidationErrors.IsNullOrEmpty())
            {
                
                var hashedPassword = _passwordHasher.HashPassword(user, password);
                user.PasswordHash = hashedPassword;
                
                await _usersRepository.AddAsync(user);
            }
            else
            {
                throw new PasswordValidationException(string.Join("\n", pwdValidationErrors));
            }            
        }
    }

    public interface IRegisterService
    {
        public Task RegisterAsync(string username, string password);
    }
}
