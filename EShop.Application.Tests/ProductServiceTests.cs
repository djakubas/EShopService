using EShop.Application.Services;
using EShop.Domain.Models.Products;
using EShop.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Application.Tests
{
    public class ProductServiceTests
    {

        // GetAllAsync tests

        [Fact]
        public async Task GetAllAsync_ValidInput_ReturnsProducts()
        {
            // Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            var productService = new ProductService(productRepositoryMock.Object);
            var products = new List<Product> { new Product { Id = 1, Price = 10.0m } };
            productRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

            // Act
            var result = await productService.GetAllAsync();

            // Assert
            Assert.Equal(1, result.Count());
        }

        [Fact]
        public async Task GetAllAsync_EmptyInput_ReturnsEmpty()
        {
            // Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            var productService = new ProductService(productRepositoryMock.Object);
            productRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Product>());

            // Act
            var result = await productService.GetAllAsync();

            // Assert
            Assert.Empty(result);
        }





        // GetByIdAsync tests

        [Fact]
        public async Task GetByIdAsync_ValidInput_ReturnsProduct()
        {
            // Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            var productService = new ProductService(productRepositoryMock.Object);
            var product = new Product { Id = 1, Price = 10.0m };
            productRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(product);

            // Act
            var result = await productService.GetByIdAsync(1);

            // Assert
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_EmptyInput_ReturnsNull()
        {
            // Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            var productService = new ProductService(productRepositoryMock.Object);
            productRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Product)null);

            // Act
            var result = await productService.GetByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        // AddAsync tests
        [Fact]
        public async Task AddAsync_ValidInput_ReturnsAddedProduct()
        {
            // Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            var productService = new ProductService(productRepositoryMock.Object);
            var product = new Product { Id = 1, Price = 10.0m };
            productRepositoryMock.Setup(repo => repo.AddAsync(product)).ReturnsAsync(product);

            // Act
            var result = await productService.AddAsync(product);

            // Assert
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task AddAsync_NullInput_ThrowsArgumentNullException()
        {
            // Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            var productService = new ProductService(productRepositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => productService.AddAsync(null));
        }





        // UpdateAsync tests

        [Fact]
        public async Task UpdateAsync_ValidInput_ReturnsUpdatedProduct()
        {
            // Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            var productService = new ProductService(productRepositoryMock.Object);
            var product = new Product { Id = 1, Price = 10.0m };
            productRepositoryMock.Setup(repo => repo.UpdateAsync(product)).ReturnsAsync(product);

            // Act
            var result = await productService.UpdateAsync(product);

            // Assert
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task UpdateAsync_NullInput_ThrowsArgumentNullException()
        {
            // Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            var productService = new ProductService(productRepositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => productService.UpdateAsync(null));
        }

    }
}
