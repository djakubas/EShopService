using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application;
using User.Domain.Exceptions;
using UserService.Controllers;
using User.Domain.Models;


namespace UserService.Tests;
public class RegisterControllerTests
{
    private readonly Mock<IRegisterService> _registerServiceMock;
    private readonly RegisterController _controller;

    public RegisterControllerTests()
    {
        _registerServiceMock = new Mock<IRegisterService>();
        _controller = new RegisterController(_registerServiceMock.Object);
    }

    [Fact]
    public async Task Register_ValidRequest_ReturnsOk()
    {
        // Arrange
        var loginRequest = new LoginRequest { Username = "user", Password = "pass123" };
        _registerServiceMock.Setup(s => s.RegisterAsync(loginRequest.Username, loginRequest.Password)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Register(loginRequest) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task Register_PasswordValidationException_ReturnsBadRequest()
    {
        // Arrange
        var loginRequest = new LoginRequest { Username = "user", Password = "weak" };
        _registerServiceMock.Setup(s => s.RegisterAsync(loginRequest.Username, loginRequest.Password)).ThrowsAsync(new PasswordValidationException());

        // Act
        var result = await _controller.Register(loginRequest) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
    }

    [Fact]
    public async Task Register_UserAlreadyExistsException_ReturnsBadRequest()
    {
        // Arrange
        var loginRequest = new LoginRequest { Username = "user", Password = "pass123" };
        _registerServiceMock.Setup(s => s.RegisterAsync(loginRequest.Username, loginRequest.Password)).ThrowsAsync(new UserAlreadyExistsException());

        // Act
        var result = await _controller.Register(loginRequest) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
    }
}
