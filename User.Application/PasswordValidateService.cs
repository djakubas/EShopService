using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace User.Application
{
    public class PasswordValidateService
    {
        private readonly IdentityOptions _identityOptions;
        public PasswordValidateService(IdentityOptions identityOptions)
        {
            _identityOptions = identityOptions;
        }

        public List<string> ValidatePassword(string password)
    {
        var errors = new List<string>();

        if (password.Length < _identityOptions.Password.RequiredLength)
            errors.Add($"Password must be at least {_identityOptions.Password.RequiredLength} characters long.");

        if (_identityOptions.Password.RequireNonAlphanumeric && password.All(char.IsLetterOrDigit))
            errors.Add("Password must contain at least one non-alphanumeric character.");

        if (_identityOptions.Password.RequireDigit && !password.Any(char.IsDigit))
            errors.Add("Password must contain at least one digit.");

        if (_identityOptions.Password.RequireUppercase && !password.Any(char.IsUpper))
            errors.Add("Password must contain at least one uppercase letter.");

        if (_identityOptions.Password.RequireLowercase && !password.Any(char.IsLower))
            errors.Add("Password must contain at least one lowercase letter.");

        return errors;
    }
    }

    public interface IPasswordValidateService
    {
        public List<string> ValidatePassword(string password);
    }
}
