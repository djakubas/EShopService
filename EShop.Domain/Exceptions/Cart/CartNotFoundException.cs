using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.Exceptions.Cart
{
    public class CartNotFoundException : Exception
    {
        public CartNotFoundException() { }
        public CartNotFoundException(string message) : base(message) { }
    }
}
