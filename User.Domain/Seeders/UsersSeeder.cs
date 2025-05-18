using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Models;
using User.Domain.Repositories;

namespace User.Domain.Seeders
{
    public class UsersSeeder(UsersDataContext _context, IPasswordHasher<UserModel> _passwordHasher) : IUsersSeeder
    {
        public async Task Seed()
        {
            if (!_context.Users.Any())
            {
                var users = new List<UserModel>
                {
                    new UserModel
                    {
                        UserName = "Admin", PasswordHash = _passwordHasher.HashPassword(null!, "test") , Role = "Administrator"
                    },
                    new UserModel
                    {
                        UserName = "Employee", PasswordHash = _passwordHasher.HashPassword(null!, "test") , Role = "Employee"
                    },
                    new UserModel
                    {
                        UserName = "Client", PasswordHash = _passwordHasher.HashPassword(null!, "test") , Role = "Client"
                    }
                };

                _context.Users.AddRange(users);
                await _context.SaveChangesAsync();
            }
        }

    }

    public interface IUsersSeeder
    {
        public Task Seed();
    }
}
