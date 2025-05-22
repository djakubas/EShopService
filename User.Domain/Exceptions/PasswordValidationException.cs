using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Exceptions
{
    public class PasswordValidationException : Exception
    {
        public PasswordValidationException() : base("Password does not meet requirments") { }
        public PasswordValidationException(Exception innerException) : base("Password does not meet requirments", innerException) { }
        public PasswordValidationException(string message) : base(message) { }
    }
}
