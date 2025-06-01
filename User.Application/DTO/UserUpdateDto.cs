using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Models;

namespace User.Application.DTO
{
    public class UserUpdateDto
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
    }
}
