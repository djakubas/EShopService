using AutoMapper;
using EShop.Application.Dto;
using EShop.Application.Services;
using EShop.Domain.Models.Products;
using EShopService.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShopService.Tests;
public class ProductControllerTests
{
    private readonly Mock<IProductService> _productServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ProductController _controller;

    public ProductControllerTests()
    {
        _productServiceMock = new Mock<IProductService>();
        _mapperMock = new Mock<IMapper>();
        _controller = new ProductController(_productServiceMock.Object, _mapperMock.Object);
    }





    // Get Tests

    [Fact]
    public async Task Get_ProductsExist_ReturnsOk()
    {
        // Arrange
        var products = new List<Product> { new Product { Id = 1, Name = "Laptop" } };
        var productDtos = new List<ProductDto> { new ProductDto { Id = 1, Name = "Laptop" } };
        _productServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(products);
        _mapperMock.Setup(m => m.Map<List<ProductDto>>(products)).Returns(productDtos);

        // Act
        var result = await _controller.Get() as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task Get_NoProducts_ReturnsNotFound()
    {
        // Arrange
        _productServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<Product>());

        // Act
        var result = await _controller.Get() as NotFoundObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
    }






    // Get(id) Tests

    [Fact]
    public async Task Get_ExistingId_ReturnsOk()
    {
        // Arrange
        var id = 1;
        var product = new Product { Id = id, Name = "Laptop" };
        var productDto = new ProductDto { Id = id, Name = "Laptop" };
        _productServiceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(product);
        _mapperMock.Setup(m => m.Map<ProductDto>(product)).Returns(productDto);

        // Act
        var result = await _controller.Get(id) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task Get_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var id = 999;
        _productServiceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((Product?)null);

        // Act
        var result = await _controller.Get(id) as NotFoundObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
    }






    // Post Tests

    [Fact]
    public async Task Post_ValidProduct_ReturnsOk()
    {
        // Arrange
        var productDto = new ProductDto { Id = 1, Name = "Laptop" };
        var product = new Product { Id = 1, Name = "Laptop" };
        _mapperMock.Setup(m => m.Map<Product>(productDto)).Returns(product);
        _productServiceMock.Setup(s => s.AddAsync(product)).ReturnsAsync(product);

        // Act
        var result = await _controller.Post(productDto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task Post_NullProduct_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _controller.Post(null!));
    }






    //Put Tests

    [Fact]
    public async Task Put_ValidProduct_ReturnsOk()
    {
        // Arrange
        var id = 1;
        var productDto = new ProductDto { Id = id, Name = "Laptop" };
        var product = new Product { Id = id, Name = "Laptop" };
        _mapperMock.Setup(m => m.Map<Product>(productDto)).Returns(product);
        _productServiceMock.Setup(s => s.UpdateAsync(product)).ReturnsAsync(product);

        // Act
        var result = await _controller.Put(id, productDto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }
    
    [Fact]
    public async Task Put_NullProduct_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _controller.Put(1, null!));
    }

    [Fact]
    public async Task Delete_ValidId_ReturnsOk()
    {
        // Arrange
        var id = 1;
        var product = new Product { Id = id, Name = "Laptop" };
        _productServiceMock.Setup(s => s.DeleteAsync(id)).ReturnsAsync(product);

        // Act
        var result = await _controller.Delete(id) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }
}