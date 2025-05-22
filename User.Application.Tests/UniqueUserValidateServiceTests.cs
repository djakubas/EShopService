using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using User.Domain.Models;
using User.Domain.Repositories;
using Xunit.Sdk;
namespace User.Application.Tests
{
    public class UniqueUserValidateServiceTests
    {
        [Fact]
        public async Task CheckUniqueUserReturnTrue()
        {

            var _usersRepositoryMock = new Mock<IUsersRepository>();
            var _uniqueUserValidateService = new UniqueUserValidateService(_usersRepositoryMock.Object);

            string username = "uniqueUser";
            _usersRepositoryMock.Setup(p => p.GetUserAsync("uniqueUser")).ReturnsAsync((UserModel)null!);

            var result = await _uniqueUserValidateService.CheckUniqueUsername(username);

            Assert.True(result);
        }
        [Fact]
        public async Task CheckNonUniqueUserReturnFalse()
        {

            var _usersRepositoryMock = new Mock<IUsersRepository>();
            var _uniqueUserValidateService = new UniqueUserValidateService(_usersRepositoryMock.Object);

            string username = "nonUniqueUser";
            UserModel user = new() { UserName = username };
            _usersRepositoryMock.Setup(p => p.GetUserAsync("nonUniqueUser")).ReturnsAsync(user);

            var result = await _uniqueUserValidateService.CheckUniqueUsername(username);

            Assert.False(result);
            Assert.Equal(username, user.UserName);
        }


    }
}
