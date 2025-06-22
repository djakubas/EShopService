using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using User.Application;
using User.Domain.Exceptions;
using UserService.Controllers;
using User.Domain.Models;

namespace UserService.Tests;
public class LoginControllerTests
{
    private readonly Mock<ILogger<LoginController>> _loggerMock;
    private readonly Mock<ILoginService> _loginServiceMock;
    private readonly LoginController _controller;

    public LoginControllerTests()
    {
        _loggerMock = new Mock<ILogger<LoginController>>();
        _loginServiceMock = new Mock<ILoginService>();
        _controller = new LoginController(_loggerMock.Object, _loginServiceMock.Object);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsOk()
    {
        // Arrange
        var loginRequest = new LoginRequest { Username = "user", Password = "pass" };
        _loginServiceMock.Setup(s => s.Login(loginRequest.Username, loginRequest.Password)).ReturnsAsync("token123");

        // Act
        var result = await _controller.Login(loginRequest) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var loginRequest = new LoginRequest { Username = "user", Password = "wrong" };
        _loginServiceMock.Setup(s => s.Login(loginRequest.Username, loginRequest.Password)).ThrowsAsync(new InvalidCredentialsException());

        // Act
        var result = await _controller.Login(loginRequest) as UnauthorizedObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(401, result.StatusCode);
    }

    [Fact]
    public void AdminPage_AuthorizedAdmin_ReturnsOk()
    {
        // Arrange
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new System.Security.Claims.ClaimsPrincipal(new System.Security.Claims.ClaimsIdentity(new[] { new System.Security.Claims.Claim(ClaimTypes.Role, "Administrator") })) }
        };

        // Act
        var result = _controller.AdminPage() as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public void EmployeePage_AuthorizedEmployee_ReturnsOk()
    {
        // Arrange
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new System.Security.Claims.ClaimsPrincipal(new System.Security.Claims.ClaimsIdentity(new[] { new System.Security.Claims.Claim(ClaimTypes.Role, "Employee") })) }
        };

        // Act
        var result = _controller.EmployeePage() as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public void ClientPage_AuthorizedClient_ReturnsOk()
    {
        // Arrange
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new System.Security.Claims.ClaimsPrincipal(new System.Security.Claims.ClaimsIdentity(new[] { new System.Security.Claims.Claim(ClaimTypes.Role, "Client") })) }
        };

        // Act
        var result = _controller.ClientPage() as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }
}
