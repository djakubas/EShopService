using EShop.Application.Services;
using EShop.Domain.Exceptions.Cart;
using EShop.Domain.Exceptions.Products;
using EShop.Domain.Models.Cart;
using EShopService.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShopService.Tests;
public class CartControllerTests
{
    // AddItemToCart tests

    [Fact]
    public async Task AddItemToCart_ValidInput_ReturnsOk()
    {
        // Arrange
        var mockService = new Mock<ICartService>();
        var cart = new Cart { Id = Guid.NewGuid() };
        mockService.Setup(s => s.AddItemToCartAsync(1, cart.Id)).ReturnsAsync(cart);
        var controller = new CartController(mockService.Object);

        // Act
        var result = await controller.AddItemToCart(1, 1, cart.Id) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task AddItemToCart_ProductNotFound_ReturnsNotFound()
    {
        // Arrange
        var mockService = new Mock<ICartService>();
        mockService.Setup(s => s.AddItemToCartAsync(1, It.IsAny<Guid>())).ThrowsAsync(new ProductNotFoundException());
        var controller = new CartController(mockService.Object);

        // Act
        var result = await controller.AddItemToCart(1, 1, Guid.NewGuid()) as NotFoundObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task AddItemToCart_CartNotFound_ReturnsNotFound()
    {
        // Arrange
        var mockService = new Mock<ICartService>();
        mockService.Setup(s => s.AddItemToCartAsync(1, It.IsAny<Guid>())).ThrowsAsync(new CartNotFoundException());
        var controller = new CartController(mockService.Object);

        // Act
        var result = await controller.AddItemToCart(1, 1, Guid.NewGuid()) as NotFoundObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
    }







    // RemoveItemFromCart tests

    [Fact]
    public async Task RemoveItemFromCart_ValidInput_ReturnsOk()
    {
        // Arrange
        var mockService = new Mock<ICartService>();
        var cart = new Cart { Id = Guid.NewGuid() };
        mockService.Setup(s => s.RemoveItemFromCartAsync(1, cart.Id)).ReturnsAsync(cart);
        var controller = new CartController(mockService.Object);

        // Act
        var result = await controller.RemoveItemFromCart(1, cart.Id) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task RemoveItemFromCart_ProductNotFound_ReturnsNotFound()
    {
        // Arrange
        var mockService = new Mock<ICartService>();
        mockService.Setup(s => s.RemoveItemFromCartAsync(1, It.IsAny<Guid>())).ThrowsAsync(new ProductNotFoundException());
        var controller = new CartController(mockService.Object);

        // Act
        var result = await controller.RemoveItemFromCart(1, Guid.NewGuid()) as NotFoundObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task RemoveItemFromCart_CartNotFound_ReturnsNotFound()
    {
        // Arrange
        var mockService = new Mock<ICartService>();
        mockService.Setup(s => s.RemoveItemFromCartAsync(1, It.IsAny<Guid>())).ThrowsAsync(new CartNotFoundException());
        var controller = new CartController(mockService.Object);

        // Act
        var result = await controller.RemoveItemFromCart(1, Guid.NewGuid()) as NotFoundObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
    }




    // CalculateCart tests

    [Fact]
    public async Task CalculateCart_ValidInput_ReturnsOk()
    {
        // Arrange
        var mockService = new Mock<ICartService>();
        var cart = new Cart { Id = Guid.NewGuid() };
        mockService.Setup(s => s.CalculateCartAsync(cart.Id)).ReturnsAsync(cart);
        var controller = new CartController(mockService.Object);

        // Act
        var result = await controller.CalculateCart(cart.Id) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task CalculateCart_CartNotFound_ReturnsNotFound()
    {
        // Arrange
        var mockService = new Mock<ICartService>();
        mockService.Setup(s => s.CalculateCartAsync(It.IsAny<Guid>())).ThrowsAsync(new CartNotFoundException());
        var controller = new CartController(mockService.Object);

        // Act
        var result = await controller.CalculateCart(Guid.NewGuid()) as NotFoundObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
    }
}

