using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.InMemory;
using EShop.Domain.Models.Products;
using EShop.Domain.Models.Cart;

namespace EShop.Domain.Repositories
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //optionsBuilder.UseSqlServer("CONN STRING");
        //    //optionsBuilder.UseInMemoryDatabase("TestDataBase");
        //}
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
    }
}
