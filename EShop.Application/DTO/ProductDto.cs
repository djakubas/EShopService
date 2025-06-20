﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Domain.Models.Products;

namespace EShop.Application.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public Category? Category { get; set; }
    }
}
