using EShop.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace EShop.Domain.Seeders
{
    public class EShopSeeder(DataContext context) : IEShopSeeder
    {
        public async Task Seed()
        {
            if (!context.Products.Any())
            {
                var Products = new List<Product>
                {
                    new Product
                    {
                        Id = 1,Name = "Laptop Lenovo ThinkPad X1",Ean = "1234567890123",Price = 5999.99m,Stock = 15,Sku = "LEN-X1-001",Category = new Category {Name = "Electronics" }
                    },
                    new Product
                    {
                        Id = 2,Name = "Smartfon Samsung Galaxy S24",Ean = "2345678901234",Price = 4299.00m,Stock = 25,Sku = "SAM-S24-002",Category = new Category {Name = "Electronics" }
                    },
                    new Product
                    {
                        Id = 3,Name = "Fotel ergonomiczny ErgoChair",Ean = "3456789012345",Price = 1249.50m,Stock = 8,Sku = "ERG-CHR-003",Category = new Category {Name = "Office" }
                    }
                };

                context.Products.AddRange(Products);
                await context.SaveChangesAsync();
            }
        }

        public async Task CleanProductsTable()
        {
            context.Products.RemoveRange(context.Products);
            await context.SaveChangesAsync();
        }
    }

    public interface IEShopSeeder
    {
        public Task Seed();
        public Task CleanProductsTable();
    }
}
