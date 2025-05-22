using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Exceptions
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException() : base("User with this username already exists") { }
        public UserAlreadyExistsException(Exception innerException) : base("User with this username already exists",innerException) { }
    }
}
