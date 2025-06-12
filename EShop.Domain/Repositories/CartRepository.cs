using EShop.Domain.Cart;
using EShop.Domain.Models;
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
        public async Task<IEnumerable<CartModel>> GetAllAsync()
        {
            return await _context.Carts.ToListAsync();
        }
        public async Task<CartModel?> GetByIdAsync(Guid id)
        {
            return await _context.Carts.Where(c => (c.Id == id)).FirstOrDefaultAsync();
        }
        public async Task<CartModel> AddAsync(CartModel cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
        }
        public async Task<CartModel> UpdateAsync(CartModel cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
            return cart;
        }
        public async Task<CartModel> DeleteAsync(CartModel cart)
        {
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return cart;
        }
    }
    public interface ICartRepository
    {
        Task<IEnumerable<CartModel>> GetAllAsync();
        Task<CartModel?> GetByIdAsync(Guid id);
        Task<CartModel> AddAsync(CartModel cart);
        Task<CartModel> UpdateAsync(CartModel cart);
        Task<CartModel> DeleteAsync(CartModel cart);
    }
}
