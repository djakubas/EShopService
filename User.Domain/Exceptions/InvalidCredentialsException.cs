using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Exceptions
{
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException() : base("Invalid Credentials") { }
        public InvalidCredentialsException(Exception innerException) : base("Invalid Credentials", innerException) { }
    }
}
