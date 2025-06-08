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
        public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
        public decimal? Price { get; set; }
        public decimal? Tax { get; set; }
    }
}
