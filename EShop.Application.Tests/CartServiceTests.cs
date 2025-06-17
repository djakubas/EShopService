using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EShop.Application.Services;
using EShop.Domain.Models.Products;
using EShop.Domain.Models.Cart;
using EShop.Domain.Repositories;
using Moq;
using Xunit;
using EShop.Domain.Exceptions.Cart;
using EShop.Domain.Exceptions.Products;

namespace EShop.Application.Tests
{
    public class CartServiceTests
    {
        // CalculateCartAsync tests

        [Fact]
        public async Task CalculateCartAsync_ValidCartWithItems_ReturnsPriceTotal()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var cartRepositoryMock = new Mock<ICartRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var cartService = new CartService(cartRepositoryMock.Object, productRepositoryMock.Object);
            var cart = new Cart
            {
                Id = cartId,
                Items = new List<CartItem>
                {
                    new CartItem { Id = 1, Quantity = 2 },
                    new CartItem { Id = 2, Quantity = 1 }
                }
            };
            cartRepositoryMock.Setup(repo => repo.GetByIdAsync(cartId)).ReturnsAsync(cart);
            productRepositoryMock.Setup(repo => repo.GetByIdsAsync(It.IsAny<List<int>>()))
                .ReturnsAsync(new List<Product>
                {
                    new Product { Id = 1, Price = 10.0m },
                    new Product { Id = 2, Price = 20.0m }
                });
            // Act
            var result = await cartService.CalculateCartAsync(cartId);
            // Assert
            Assert.Equal(40.0m, result.PriceTotal);
        }

        [Fact]
        public async Task CalculateCartAsync_ValidCartButEmpty_ReturnsZero()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var cartRepositoryMock = new Mock<ICartRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var cartService = new CartService(cartRepositoryMock.Object, productRepositoryMock.Object);

            var emptyCart = new Cart
            {
                Id = cartId,
                Items = new List<CartItem>() // Pusty koszyk
            };

            cartRepositoryMock.Setup(repo => repo.GetByIdAsync(cartId)).ReturnsAsync(emptyCart);

            // Act
            var result = await cartService.CalculateCartAsync(cartId);

            // Assert
            Assert.Equal(0.0m, result.PriceTotal);
        }

        [Fact]
        public async Task CalculateCartAsync_NonExistingCart_ThrowsCartNotFoundException()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var cartRepositoryMock = new Mock<ICartRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var cartService = new CartService(cartRepositoryMock.Object, productRepositoryMock.Object);
           
            cartRepositoryMock.Setup(repo => repo.GetByIdAsync(cartId)).ReturnsAsync((Cart)null);
            
            // Act & Assert
            await Assert.ThrowsAsync<CartNotFoundException>(() => cartService.CalculateCartAsync(cartId));
        }

        [Fact]
        public async Task CalculateCartAsync_NonExistingProduct_ThrowsProductNotFoundException()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var cartRepositoryMock = new Mock<ICartRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var cartService = new CartService(cartRepositoryMock.Object, productRepositoryMock.Object);

            var cart = new Cart
            {
                Id = cartId,
                Items = new List<CartItem> { new CartItem { Id = 1, Quantity = 2 } }
            };
            cartRepositoryMock.Setup(repo => repo.GetByIdAsync(cartId)).ReturnsAsync(cart);
            productRepositoryMock.Setup(repo => repo.GetByIdsAsync(It.IsAny<List<int>>()))
                .ReturnsAsync(new List<Product>()); 

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ProductNotFoundException>(() => cartService.CalculateCartAsync(cartId));
            Assert.Equal("Product with ID 1 not found.", exception.Message);
        }




        // AddItemToCartAsync tests

        [Fact]
        public async Task AddItemToCartAsync_NonExistingCart_ThrowsCartNotFoundException()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var productId = 1;
            var cartRepositoryMock = new Mock<ICartRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var cartService = new CartService(cartRepositoryMock.Object, productRepositoryMock.Object);

            cartRepositoryMock.Setup(repo => repo.GetByIdAsync(cartId)).ReturnsAsync((Cart)null);

            // Act & Assert
            await Assert.ThrowsAsync<CartNotFoundException>(() => cartService.AddItemToCartAsync(productId, cartId));
        }

        [Fact]
        public async Task AddItemToCartAsync_NonExistingProduct_ThrowsProductNotFoundException()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var productId = 1;
            var cartRepositoryMock = new Mock<ICartRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var cartService = new CartService(cartRepositoryMock.Object, productRepositoryMock.Object);

            var cart = new Cart { Id = cartId, Items = new List<CartItem>() };
            cartRepositoryMock.Setup(repo => repo.GetByIdAsync(cartId)).ReturnsAsync(cart);
            productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync((Product)null);

            // Act & Assert
            await Assert.ThrowsAsync<ProductNotFoundException>(() => cartService.AddItemToCartAsync(productId, cartId));
        }

        [Fact]
        public async Task AddItemToCartAsync_AddingToEmptyCart_SetsQuantityToOne()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var productId = 1;
            var cartRepositoryMock = new Mock<ICartRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var cartService = new CartService(cartRepositoryMock.Object, productRepositoryMock.Object);

            var cart = new Cart { Id = cartId, Items = new List<CartItem>() };
            var product = new Product { Id = productId };

            cartRepositoryMock.Setup(repo => repo.GetByIdAsync(cartId)).ReturnsAsync(cart);
            productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(product);
            cartRepositoryMock.Setup(repo => repo.UpdateAsync(cart)).ReturnsAsync(cart);

            // Act
            var result = await cartService.AddItemToCartAsync(productId, cartId);

            // Assert
            Assert.Equal(1, result.Items.FirstOrDefault(i => i.Id == productId)?.Quantity);
        }

        [Fact]
        public async Task AddItemToCartAsync_ItemExists_IncrementsQuantity()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var productId = 1;
            var initialQuantity = 1;
            var cartRepositoryMock = new Mock<ICartRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var cartService = new CartService(cartRepositoryMock.Object, productRepositoryMock.Object);

            var cart = new Cart
            {
                Id = cartId,
                Items = new List<CartItem> { new CartItem { Id = productId, Quantity = initialQuantity } }
            };
            var product = new Product { Id = productId };

            cartRepositoryMock.Setup(repo => repo.GetByIdAsync(cartId)).ReturnsAsync(cart);
            productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(product);
            cartRepositoryMock.Setup(repo => repo.UpdateAsync(cart)).ReturnsAsync(cart);

            // Act
            var result = await cartService.AddItemToCartAsync(productId, cartId);

            // Assert
            Assert.Equal(initialQuantity + 1, result.Items.FirstOrDefault(i => i.Id == productId)?.Quantity);
        }




        // RemoveItemFromCartAsync tests

        [Fact]
        public async Task RemoveItemFromCartAsync_NonExistingCart_ThrowsCartNotFoundException()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var productId = 1;
            var cartRepositoryMock = new Mock<ICartRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var cartService = new CartService(cartRepositoryMock.Object, productRepositoryMock.Object);

            cartRepositoryMock.Setup(repo => repo.GetByIdAsync(cartId)).ReturnsAsync((Cart)null);

            // Act & Assert
            await Assert.ThrowsAsync<CartNotFoundException>(() => cartService.RemoveItemFromCartAsync(productId, cartId));
        }

        [Fact]
        public async Task RemoveItemFromCartAsync_NonExistingItem_ThrowsProductNotFoundException()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var productId = 1;
            var cartRepositoryMock = new Mock<ICartRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var cartService = new CartService(cartRepositoryMock.Object, productRepositoryMock.Object);

            var cart = new Cart { Id = cartId, Items = new List<CartItem>() };
            cartRepositoryMock.Setup(repo => repo.GetByIdAsync(cartId)).ReturnsAsync(cart);

            // Act & Assert
            await Assert.ThrowsAsync<ProductNotFoundException>(() => cartService.RemoveItemFromCartAsync(productId, cartId));
        }

        [Fact]
        public async Task RemoveItemFromCartAsync_QuantityOne_RemovesItem()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var productId = 1;
            var cartRepositoryMock = new Mock<ICartRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var cartService = new CartService(cartRepositoryMock.Object, productRepositoryMock.Object);

            var cart = new Cart
            {
                Id = cartId,
                Items = new List<CartItem> { new CartItem { Id = productId, Quantity = 1 } }
            };
            cartRepositoryMock.Setup(repo => repo.GetByIdAsync(cartId)).ReturnsAsync(cart);
            cartRepositoryMock.Setup(repo => repo.UpdateAsync(cart)).ReturnsAsync(cart);

            // Act
            var result = await cartService.RemoveItemFromCartAsync(productId, cartId);

            // Assert
            Assert.Null(result.Items.FirstOrDefault(i => i.Id == productId));
        }

        [Fact]
        public async Task RemoveItemFromCartAsync_QuantityGreaterThanOne_DecrementsQuantity()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var productId = 1;
            var initialQuantity = 2;
            var cartRepositoryMock = new Mock<ICartRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var cartService = new CartService(cartRepositoryMock.Object, productRepositoryMock.Object);

            var cart = new Cart
            {
                Id = cartId,
                Items = new List<CartItem> { new CartItem { Id = productId, Quantity = initialQuantity } }
            };
            cartRepositoryMock.Setup(repo => repo.GetByIdAsync(cartId)).ReturnsAsync(cart);
            cartRepositoryMock.Setup(repo => repo.UpdateAsync(cart)).ReturnsAsync(cart);

            // Act
            var result = await cartService.RemoveItemFromCartAsync(productId, cartId);

            // Assert
            Assert.Equal(initialQuantity - 1, result.Items.FirstOrDefault(i => i.Id == productId)?.Quantity);
        }




        // CleanCartAsync tests

        [Fact]
        public async Task CleanCartAsync_NonExistingCart_ThrowsCartNotFoundException()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var productId = 1;
            var cartRepositoryMock = new Mock<ICartRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var cartService = new CartService(cartRepositoryMock.Object, productRepositoryMock.Object);

            cartRepositoryMock.Setup(repo => repo.GetByIdAsync(cartId)).ReturnsAsync((Cart)null);

            // Act & Assert
            await Assert.ThrowsAsync<CartNotFoundException>(() => cartService.CleanCartAsync(productId, cartId));
        }

        [Fact]
        public async Task CleanCartAsync_CartWithProducts_ClearsAllItems()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var productId = 1;
            var cartRepositoryMock = new Mock<ICartRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var cartService = new CartService(cartRepositoryMock.Object, productRepositoryMock.Object);

            var cart = new Cart
            {
                Id = cartId,
                Items = new List<CartItem> { new CartItem { Id = productId, Quantity = 2 } }
            };
            cartRepositoryMock.Setup(repo => repo.GetByIdAsync(cartId)).ReturnsAsync(cart);
            cartRepositoryMock.Setup(repo => repo.UpdateAsync(cart)).ReturnsAsync(cart);

            // Act
            var result = await cartService.CleanCartAsync(productId, cartId);

            // Assert
            Assert.Empty(result.Items);
        }
    }
}
