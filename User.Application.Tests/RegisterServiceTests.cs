using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Exceptions;
using User.Domain.Models;
using User.Domain.Repositories;

namespace User.Application.Tests;
public class RegisterServiceTests
{
    ///*
    [Fact]
    public async Task RegisterAsync_ValidInput_CallsAddAsyncOnce()
    {
        // Arrange
        var usersRepositoryMock = new Mock<IUsersRepository>();
        var passwordHasherMock = new Mock<IPasswordHasher<UserModel>>();
        var passwordValidatorMock = new Mock<IPasswordValidateService>();
        var uniqueUserValidateServiceMock = new Mock<IUniqueUserValidateService>();

        var username = "newuser";
        var password = "ValidPass123!";
        var hashedPassword = "hashedpassword123";
        var user = new UserModel { UserName = username, PasswordHash = hashedPassword, Role = "Client" };

        uniqueUserValidateServiceMock.Setup(u => u.CheckUniqueUsername(username)).ReturnsAsync(true);
        passwordValidatorMock.Setup(p => p.ValidatePassword(password)).Returns(new List<string>());
        passwordHasherMock.Setup(p => p.HashPassword(user, password)).Returns(hashedPassword);

        var registerService = new RegisterService(usersRepositoryMock.Object, passwordHasherMock.Object, passwordValidatorMock.Object, uniqueUserValidateServiceMock.Object);

        // Act
        await registerService.RegisterAsync(username, password);

        // Assert
    }

    //*/
    [Fact]
    public async Task RegisterAsync_UserAlreadyExists_ThrowsUserAlreadyExistsException()
    {
        // Arrange
        var usersRepositoryMock = new Mock<IUsersRepository>();
        var passwordHasherMock = new Mock<IPasswordHasher<UserModel>>();
        var passwordValidatorMock = new Mock<IPasswordValidateService>();
        var uniqueUserValidateServiceMock = new Mock<IUniqueUserValidateService>();

        var username = "existinguser";
        var password = "ValidPass123!";

        uniqueUserValidateServiceMock.Setup(u => u.CheckUniqueUsername(username)).ReturnsAsync(false);

        var registerService = new RegisterService(usersRepositoryMock.Object, passwordHasherMock.Object, passwordValidatorMock.Object, uniqueUserValidateServiceMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UserAlreadyExistsException>(() => registerService.RegisterAsync(username, password));
    }

    [Fact]
    public async Task RegisterAsync_InvalidPassword_ThrowsPasswordValidationException()
    {
        // Arrange
        var usersRepositoryMock = new Mock<IUsersRepository>();
        var passwordHasherMock = new Mock<IPasswordHasher<UserModel>>();
        var passwordValidatorMock = new Mock<IPasswordValidateService>();
        var uniqueUserValidateServiceMock = new Mock<IUniqueUserValidateService>();

        var username = "newuser";
        var password = "weak";
        var validationErrors = new List<string> { "Password must contain at least one digit.", "Password must contain at least one uppercase letter." };

        uniqueUserValidateServiceMock.Setup(u => u.CheckUniqueUsername(username)).ReturnsAsync(true);
        passwordValidatorMock.Setup(p => p.ValidatePassword(password)).Returns(validationErrors);

        var registerService = new RegisterService(usersRepositoryMock.Object, passwordHasherMock.Object, passwordValidatorMock.Object, uniqueUserValidateServiceMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<PasswordValidationException>(() => registerService.RegisterAsync(username, password));
    }
}