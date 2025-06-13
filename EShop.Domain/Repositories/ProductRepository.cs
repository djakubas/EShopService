using EShop.Domain.Models.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await _context.Products.Where(p => ids.Contains(p.Id)).ToListAsync();
        }
        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.Where(p => (p.Id == id)).FirstOrDefaultAsync();
        }
        public async Task<Product> AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }
        public async Task<Product> UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }
        public async Task<Product> DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return product;
        }


    }
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<int> ids);
        Task<Product> AddAsync(Product product);
        Task<Product> UpdateAsync(Product product);
        Task<Product> DeleteAsync(Product product);
    }
}
