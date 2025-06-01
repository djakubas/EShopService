using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Models;
namespace User.Domain.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private protected UsersDataContext _context;
        public UsersRepository(UsersDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserModel>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<UserModel> GetUserAsync(Guid id)
        {
            var user = await _context.Users.Where(u => u.Id == id.ToString()).FirstOrDefaultAsync();
            return user!;
        }

        public async Task<UserModel> GetUserAsync(string username)
        {
            var user = await _context.Users.Where(u => u.UserName == username).FirstOrDefaultAsync();
            return user!;
        }
        public async Task<UserModel> AddAsync(UserModel user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<UserModel> UpdateAsync(UserModel user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<UserModel> DeleteAsync(UserModel user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }

        
    }

    public interface IUsersRepository
    {
        public Task<IEnumerable<UserModel>> GetAllAsync();
        public Task<UserModel> GetUserAsync(Guid id);
        public Task<UserModel> GetUserAsync(string username);
        public Task<UserModel> AddAsync(UserModel user);
        public Task<UserModel> UpdateAsync(UserModel user);
        public Task<UserModel> DeleteAsync(UserModel user);
    }
}
