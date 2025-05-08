using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using User.Domain.Models;

namespace User.Domain.Models
{
    public class UserModel : IdentityUser
    {
        public string Role { get; set; } = default!;
    }
}