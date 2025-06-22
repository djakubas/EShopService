using EShop.Application.Services;
using EShop.Domain.Exceptions.Products;
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
        public async Task GetAllAsync_ValidInput_ReturnsAllProducts()
        {
            // Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            var productService = new ProductService(productRepositoryMock.Object);
            var products = new List<Product> { new Product { Id = 1, Price = 10.0m }, new Product { Id = 2, Price = 20.0m } };
            productRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

            // Act
            var result = await productService.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
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
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_EmptyInput_ReturnsNull()
        {
            // Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            var productService = new ProductService(productRepositoryMock.Object);
            productRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Product)null!);

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
            await Assert.ThrowsAsync<ArgumentNullException>(() => productService.AddAsync(null!));
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
            await Assert.ThrowsAsync<ArgumentNullException>(() => productService.UpdateAsync(null!));
        }





        // DeleteAsync tests

        [Fact]
        public async Task DeleteAsync_ValidId_MarksAsDeletedAndReturnsProduct()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId, Name = "Laptop", Deleted = false };
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);
            repoMock.Setup(r => r.UpdateAsync(product)).ReturnsAsync(product);
            var service = new ProductService(repoMock.Object);

            // Act
            var result = await service.DeleteAsync(productId);

            // Assert
            Assert.True(result.Deleted);
            Assert.Same(product, result);
            repoMock.Verify(r => r.GetByIdAsync(productId), Times.Once());
            repoMock.Verify(r => r.UpdateAsync(product), Times.Once());
        }

        [Fact]
        public async Task DeleteAsync_NonExistingId_ThrowsProductNotFoundException()
        {
            // Arrange
            var productId = 1;
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync((Product?)null);
            var service = new ProductService(repoMock.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ProductNotFoundException>(() => service.DeleteAsync(productId));
            Assert.Equal($"Product with id {productId} not found.", exception.Message);
        }

    }
}
