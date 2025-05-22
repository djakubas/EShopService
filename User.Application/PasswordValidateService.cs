using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace User.Application
{
    public class PasswordValidateService : IPasswordValidateService
    {
        private readonly IOptions<IdentityOptions> _identityOptions;
        public PasswordValidateService(IOptions<IdentityOptions> identityOptions)
        {
            _identityOptions = identityOptions;
        }

        public List<string> ValidatePassword(string password)
    {
        var errors = new List<string>();
            
        if (password.Length < _identityOptions.Value.Password.RequiredLength)
            errors.Add($"Password must be at least {_identityOptions.Value.Password.RequiredLength} characters long.");

        if (_identityOptions.Value.Password.RequireNonAlphanumeric && password.All(char.IsLetterOrDigit))
            errors.Add("Password must contain at least one non-alphanumeric character.");

        if (_identityOptions.Value.Password.RequireDigit && !password.Any(char.IsDigit))
            errors.Add("Password must contain at least one digit.");

        if (_identityOptions.Value.Password.RequireUppercase && !password.Any(char.IsUpper))
            errors.Add("Password must contain at least one uppercase letter.");

        if (_identityOptions.Value.Password.RequireLowercase && !password.Any(char.IsLower))
            errors.Add("Password must contain at least one lowercase letter.");
            
        return errors;
    }
    }

    public interface IPasswordValidateService
    {
        public List<string> ValidatePassword(string password);
    }
}
