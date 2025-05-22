using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Repositories;

namespace User.Application
{
    public class UniqueUserValidateService : IUniqueUserValidateService
    {
        private readonly IUsersRepository _usersRepository;
        public UniqueUserValidateService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }
        public async Task<bool> CheckUniqueUsername(string username)
        {
            var user = await _usersRepository.GetUserAsync(username);
            if (user == null)
            {
                return true;
            }
            return false;
        }

    }

    public interface IUniqueUserValidateService
    {
        public Task<bool> CheckUniqueUsername(string username);
    }
}
