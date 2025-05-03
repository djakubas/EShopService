using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using User.Domain.Models;

namespace User.Domain.Repositories
{
    public class UsersDataContext : DbContext
    {
        public UsersDataContext(DbContextOptions options) : base(options) { }
        public DbSet<UserModel> Users { get; set; }
    }
}
