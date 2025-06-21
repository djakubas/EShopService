using Microsoft.AspNetCore.Identity;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Exceptions;
using User.Domain.Models;
using User.Domain.Repositories;

namespace User.Application.Tests;
public class LoginServiceTests
{
    [Fact]
    public async Task Login_ValidCredentials_ReturnsToken()
    {
        // Arrange
        var jwtTokenServiceMock = new Mock<IJwtTokenService>();
        var usersRepositoryMock = new Mock<IUsersRepository>();
        var passwordHasherMock = new Mock<IPasswordHasher<UserModel>>();

        var userId = "user123";
        var user = new UserModel { Id = userId, UserName = "testuser", PasswordHash = "hashedpassword", Role = "User" };
        var login = "testuser";
        var password = "password123";
        var token = "mocked-jwt-token";

        usersRepositoryMock.Setup(r => r.GetUserAsync(login)).ReturnsAsync(user);
        passwordHasherMock.Setup(p => p.VerifyHashedPassword(user, user.PasswordHash, password))
            .Returns(PasswordVerificationResult.Success);
        jwtTokenServiceMock.Setup(j => j.GenerateToken(userId, new List<string> { user.Role })).Returns(token);

        var loginService = new LoginService(jwtTokenServiceMock.Object, usersRepositoryMock.Object, passwordHasherMock.Object);

        // Act
        var result = await loginService.Login(login, password);

        // Assert
        Assert.Equal(token, result);
    }

    [Fact]
    public async Task Login_UserNotFound_ThrowsInvalidCredentialsException()
    {
        // Arrange
        var jwtTokenServiceMock = new Mock<IJwtTokenService>();
        var usersRepositoryMock = new Mock<IUsersRepository>();
        var passwordHasherMock = new Mock<IPasswordHasher<UserModel>>();

        var login = "nonexistent";
        var password = "password123";

        usersRepositoryMock.Setup(r => r.GetUserAsync(login)).ReturnsAsync((UserModel)null);

        var loginService = new LoginService(jwtTokenServiceMock.Object, usersRepositoryMock.Object, passwordHasherMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCredentialsException>(() => loginService.Login(login, password));
    }

    [Fact]
    public async Task Login_InvalidPassword_ThrowsInvalidCredentialsException()
    {
        // Arrange
        var jwtTokenServiceMock = new Mock<IJwtTokenService>();
        var usersRepositoryMock = new Mock<IUsersRepository>();
        var passwordHasherMock = new Mock<IPasswordHasher<UserModel>>();

        var user = new UserModel { Id = "user123", UserName = "testuser", PasswordHash = "hashedpassword", Role = "User" };
        var login = "testuser";
        var password = "wrongpass";

        usersRepositoryMock.Setup(r => r.GetUserAsync(login)).ReturnsAsync(user);
        passwordHasherMock.Setup(p => p.VerifyHashedPassword(user, user.PasswordHash, password))
            .Returns(PasswordVerificationResult.Failed);

        var loginService = new LoginService(jwtTokenServiceMock.Object, usersRepositoryMock.Object, passwordHasherMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCredentialsException>(() => loginService.Login(login, password));
    }
}
