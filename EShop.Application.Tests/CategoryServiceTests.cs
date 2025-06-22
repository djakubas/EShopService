using EShop.Application.Services;
using EShop.Domain.Models.Products;
using EShop.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Application.Tests;
public class CategoryServiceTests
{
    // GetAllAsync Tests

    [Fact]
    public async Task GetAllAsync_ValidInput_ReturnsAllCategories()
    {
        // Arrange
        var categories = new List<Category>
            {
                new Category { CategoryId = Guid.NewGuid(), Name = "Electronics" },
                new Category { CategoryId = Guid.NewGuid(), Name = "Clothing" }
            };
        var repoMock = new Mock<ICategoryRepository>();
        repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);
        var service = new CategoryService(repoMock.Object);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAllAsync_EmptyInput_ReturnsAllCategories()
    {
        // Arrange
        var repoMock = new Mock<ICategoryRepository>();
        repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Category>());
        var service = new CategoryService(repoMock.Object);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.Empty(result);
    }





    // GetByIdAsync Tests

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsCategory()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var category = new Category { CategoryId = categoryId, Name = "Electronics" };
        var repoMock = new Mock<ICategoryRepository>();
        repoMock.Setup(r => r.GetByIdAsync(categoryId)).ReturnsAsync(category);
        var service = new CategoryService(repoMock.Object);

        // Act
        var result = await service.GetByIdAsync(categoryId);

        // Assert
        Assert.Equal(categoryId, result?.CategoryId);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var repoMock = new Mock<ICategoryRepository>();
        repoMock.Setup(r => r.GetByIdAsync(categoryId)).ReturnsAsync((Category?)null);
        var service = new CategoryService(repoMock.Object);

        // Act
        var result = await service.GetByIdAsync(categoryId);

        // Assert
        Assert.Null(result);
    }





    // AddAsync Tests

    [Fact]
    public async Task AddAsync_ValidCategory_AddsAndReturnsCategory()
    {
        // Arrange
        var category = new Category { CategoryId = Guid.NewGuid(), Name = "Electronics" };
        var repoMock = new Mock<ICategoryRepository>();
        repoMock.Setup(r => r.AddAsync(category)).ReturnsAsync(category);
        var service = new CategoryService(repoMock.Object);

        // Act
        var result = await service.AddAsync(category);

        // Assert
        Assert.Same(category, result);
    }

    [Fact]
    public async Task AddAsync_NullCategory_ThrowsArgumentNullException()
    {
        // Arrange
        var repoMock = new Mock<ICategoryRepository>();
        var service = new CategoryService(repoMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => service.AddAsync(null));
    }





    // UpdateAsync Tests

    [Fact]
    public async Task UpdateAsync_ValidCategory_UpdatesAndReturnsCategory()
    {
        // Arrange
        var category = new Category { CategoryId = Guid.NewGuid(), Name = "Electronics" };
        var repoMock = new Mock<ICategoryRepository>();
        repoMock.Setup(r => r.UpdateAsync(category)).ReturnsAsync(category);
        var service = new CategoryService(repoMock.Object);

        // Act
        var result = await service.UpdateAsync(category);

        // Assert
        Assert.Same(category, result);
    }

    [Fact]
    public async Task UpdateAsync_NullCategory_ThrowsArgumentNullException()
    {
        // Arrange
        var repoMock = new Mock<ICategoryRepository>();
        var service = new CategoryService(repoMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => service.UpdateAsync(null));
    }





    // DeleteAsync Tests

    [Fact]
    public async Task DeleteAsync_ValidId_CallsDelete()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var repoMock = new Mock<ICategoryRepository>();
        repoMock.Setup(r => r.DeleteAsync(categoryId)).Returns(Task.CompletedTask);
        var service = new CategoryService(repoMock.Object);

        // Act
        await service.DeleteAsync(categoryId);

        // Assert
        repoMock.Verify(r => r.DeleteAsync(categoryId), Times.Once());
    }
}
