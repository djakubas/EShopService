using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Repositories;

namespace User.Application
{
    public class EditUserService : IEditUserService
    {
        private readonly IPasswordValidateService _passwordValidateService;
        private readonly IUsersRepository _usersRepository;
        public EditUserService(IPasswordValidateService passwordValidateService, IUsersRepository usersRepository)
        {
            _passwordValidateService = passwordValidateService;
            _usersRepository = usersRepository;
        }

        public async Task EditUser() { }

    }

    public interface IEditUserService
    {
        Task EditUser();
    }

}
