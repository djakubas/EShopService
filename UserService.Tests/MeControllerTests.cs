using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using User.Application.DTO;
using User.Domain.Repositories;
using UserService.Controllers;
using User.Domain.Models;
using User.Domain.Exceptions;

namespace UserService.Tests;

public class MeControllerTests
{
    private readonly Mock<IUsersRepository> _usersRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly MeController _controller;

    public MeControllerTests()
    {
        _usersRepoMock = new Mock<IUsersRepository>();
        _mapperMock = new Mock<IMapper>();
        _controller = new MeController(_usersRepoMock.Object, _mapperMock.Object);
    }





    // Get Tests

    [Fact]
    public async Task Get_ValidUserId_ReturnsOk()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var user = new UserModel { Id = userId, UserName = "testuser", Role = "Client" };
        var userDto = new UserDto {Id = userId, UserName = "testuser", Role = "Client" };
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, userId) })) }
        };
        _usersRepoMock.Setup(r => r.GetUserAsync(Guid.Parse(userId))).ReturnsAsync(user);
        _mapperMock.Setup(m => m.Map<UserDto>(user)).Returns(userDto);

        // Act
        var result = await _controller.Get() as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task Get_InvalidUserId_ReturnsBadRequest()
    {
        // Arrange
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "invalid") })) }
        };

        // Act
        var result = await _controller.Get() as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
    }





    // UpdateUserDetails Tests

    [Fact]
    public async Task UpdateUserDetails_ValidUpdate_ReturnsOk()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var user = new UserModel { Id = userId, UserName = "testuser", Role = "Client" };
        var userDto = new UserDto { Id = userId, UserName = "testuser", Role = "Client" };
        var updatedUser = new UserModel { Id = userId, UserName = "newuser" };
        var updateDto = new UserUpdateDto { UserName = "newuser" };

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, userId) })) }
        };
        _usersRepoMock.Setup(r => r.GetUserAsync(Guid.Parse(userId))).ReturnsAsync(user);
        _mapperMock.Setup(m => m.Map(updateDto, user)).Returns(updatedUser);
        _usersRepoMock.Setup(r => r.UpdateAsync(updatedUser)).ReturnsAsync(updatedUser);
        _mapperMock.Setup(m => m.Map<UserDto>(updatedUser)).Returns(userDto);

        // Act
        var result = await _controller.UpdateUserDetails(updateDto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task UpdateUserDetails_InvalidUserId_ReturnsBadRequest()
    {
        // Arrange
        var updateDto = new UserUpdateDto { UserName = "newuser", Email = "test@gmail.com" };
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "invalid") })) }
        };
        _usersRepoMock.Setup(r => r.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync((UserModel)null!);
        _mapperMock.Setup(m => m.Map(It.IsAny<UserUpdateDto>, It.IsAny<UserModel>())).Returns((UserModel)null!);

        // Act
        var result = await _controller.UpdateUserDetails(updateDto) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
    }
}