using Moq;
using Xunit;
using UserService.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using User.Application;
using User.Domain.Exceptions;
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
        // Set up a default HttpContext for authorization tests
        var httpContext = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
    }

}