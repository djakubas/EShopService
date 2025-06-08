using EShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.Cart
{
    public class CartModel
    {
        public int Id { get; set; }
        IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
        float? Price { get; set; }
        float? Tax { get; set; }
    }
}
