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
                        Id = 1,
                        Name = "Rower szosowy Giant Contend 3",
                        Ean = "1234567890123",
                        Price = 5999.99m,
                        Stock = 10,
                        Sku = "GIA-C3-001",
                        Category = new Category { Name = "Bicycles" }
                    },
                    new Product
                    {
                        Id = 2,
                        Name = "Rower górski Trek Marlin 7",
                        Ean = "2345678901234",
                        Price = 4299.00m,
                        Stock = 12,
                        Sku = "TRE-M7-002",
                        Category = new Category { Name = "Bicycles" }
                    },
                    new Product
                    {
                        Id = 3,
                        Name = "Kask rowerowy Kross Vento",
                        Ean = "3456789012345",
                        Price = 249.50m,
                        Stock = 30,
                        Sku = "KRO-VEN-003",
                        Category = new Category { Name = "Accessories" }
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
