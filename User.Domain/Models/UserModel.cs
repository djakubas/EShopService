using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Login { get; set; } = default!;
        public string HashPassword {  get; set; } = default!;
        public string Role { get; set; } = default!;
    }
}
