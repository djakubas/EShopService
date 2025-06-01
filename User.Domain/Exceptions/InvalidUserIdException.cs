using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Exceptions
{
    public class InvalidUserIdException : Exception
    {
        public InvalidUserIdException() : base("Invalid User ID") { }
        public InvalidUserIdException(Exception innerException) : base("Invalid User ID", innerException) { }
    }
}
