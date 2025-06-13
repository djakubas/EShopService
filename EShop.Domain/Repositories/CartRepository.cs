using EShop.Domain.Models;
using EShop.Domain.Models.Cart;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly DataContext _context;

        public CartRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Cart>> GetAllAsync()
        {
            return await _context.Carts.ToListAsync();
        }
        public async Task<Cart?> GetByIdAsync(Guid id)
        {
            return await _context.Carts.Where(c => (c.Id == id)).FirstOrDefaultAsync();
        }
        public async Task<Cart> AddAsync(Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
        }
        public async Task<Cart> UpdateAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
            return cart;
        }
        public async Task<Cart> DeleteAsync(Cart cart)
        {
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return cart;
        }
    }
    public interface ICartRepository
    {
        Task<IEnumerable<Cart>> GetAllAsync();
        Task<Cart?> GetByIdAsync(Guid id);
        Task<Cart> AddAsync(Cart cart);
        Task<Cart> UpdateAsync(Cart cart);
        Task<Cart> DeleteAsync(Cart cart);
    }
}
