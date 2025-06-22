using AutoMapper;
using EShop.Application.DTO;
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
public class CategoryControllerTests
{
    private readonly Mock<ICategoryService> _categoryServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CategoryController _controller;

    public CategoryControllerTests()
    {
        _categoryServiceMock = new Mock<ICategoryService>();
        _mapperMock = new Mock<IMapper>();
        _controller = new CategoryController(_categoryServiceMock.Object, _mapperMock.Object);
    }





    // Get Tests

    [Fact]
    public async Task Get_CategoriesExist_ReturnsOk()
    {
        // Arrange
        var categories = new List<Category> { new Category { Name = "Electronics" } };
        var categoryDtos = new List<CategoryDto> { new CategoryDto { Name = "Electronics" } };
        _categoryServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(categories);
        _mapperMock.Setup(m => m.Map<List<CategoryDto>>(categories)).Returns(categoryDtos);

        // Act
        var result = await _controller.Get() as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task Get_NoCategories_ReturnsNotFound()
    {
        // Arrange
        _categoryServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<Category>());

        // Act
        var result = await _controller.Get() as NotFoundObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
    }





    // Get(id) Tests

    [Fact]
    public async Task Get_CategoryExists_ReturnsOk()
    {
        // Arrange
        var id = Guid.NewGuid();
        var category = new Category { CategoryId = id, Name = "Electronics" };
        var categoryDto = new CategoryDto { Name = "Electronics" };
        _categoryServiceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(category);
        _mapperMock.Setup(m => m.Map<CategoryDto>(category)).Returns(categoryDto);

        // Act
        var result = await _controller.Get(id) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task Get_CategoryNotFound_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _categoryServiceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((Category?)null);

        // Act
        var result = await _controller.Get(id) as NotFoundObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
    }
    




    // Post Tests

    [Fact]
    public async Task Post_ValidCategory_ReturnsOk()
    {
        // Arrange
        var categoryDto = new CategoryDto { Name = "Electronics" };
        var category = new Category { Name = "Electronics" }; 
        _mapperMock.Setup(m => m.Map<Category>(categoryDto)).Returns(category);
        _categoryServiceMock.Setup(s => s.AddAsync(category)).ReturnsAsync(category);

        // Act
        var result = await _controller.Post(categoryDto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task Post_NullCategory_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _controller.Post(null!));
    }





    // Put Tests

    [Fact]
    public async Task Put_ValidCategory_ReturnsOk()
    {
        // Arrange
        var id = Guid.NewGuid();
        var categoryDto = new CategoryDto { Name = "Electronics" };
        var category = new Category { CategoryId = id, Name = "Electronics" };
        _mapperMock.Setup(m => m.Map<Category>(categoryDto)).Returns(category);
        _categoryServiceMock.Setup(s => s.UpdateAsync(category)).ReturnsAsync(category);

        // Act
        var result = await _controller.Put(id, categoryDto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task Put_NullCategory_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _controller.Put(Guid.NewGuid(), null!));
    }





    // Delete Tests

    [Fact]
    public async Task Delete_ValidId_ReturnsNoContent()
    {
        // Arrange
        var id = Guid.NewGuid();
        _categoryServiceMock.Setup(s => s.DeleteAsync(id)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(id) as NoContentResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(204, result.StatusCode);
    }

    [Fact]
    public async Task Delete_InvalidId_ReturnsBadRequest()
    {
        // Arrange
        var id = Guid.NewGuid();
        _categoryServiceMock.Setup(s => s.DeleteAsync(id)).ThrowsAsync(new Exception("Delete failed"));

        // Act
        var result = await _controller.Delete(id) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
    }
}
