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
        public Guid Id { get; set; } = Guid.NewGuid();
        public List<int> ProductsIds { get; set; } = new List<int>();
        public decimal? PriceTotal { get; set; }
        public decimal? Tax { get; set; }
    }
}
