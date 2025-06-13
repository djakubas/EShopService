using EShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.Models.Cart
{
    public class Cart
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal? PriceTotal { get; set; }
        public decimal? Tax { get; set; }
    }
}
