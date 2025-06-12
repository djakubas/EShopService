using EShop.Domain.Cart;
using EShop.Domain.Exceptions.Cart;
using EShop.Domain.Exceptions.Products;
using EShop.Domain.Models;
using EShop.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }
        public async Task<CartModel> CalculateCartAsync(Guid cartId)
        {
            var cart = await _cartRepository.GetByIdAsync(cartId)
                ?? throw new CartNotFoundException();

            var ids = cart.Products.Select(p => p.Id).ToList();
            var products = _productRepository.GetByIdsAsync(ids);

            foreach(var product in cart.Products)
            {
                //czy na pewno potrzebuje tutaj Produktu a nie tylko id produktu?
            }
            
            return cart;
        }

        public async Task<CartModel> AddItemToCartAsync(int productId, Guid cartId)
        {
            var product = await _productRepository.GetByIdAsync(productId)
                ?? throw new ProductNotFoundException();

            var cart = await _cartRepository.GetByIdAsync(cartId)
                ?? throw new CartNotFoundException();

            cart.Products.Append(product);
            await _cartRepository.UpdateAsync(cart);
            return cart;
        }

        public async Task<CartModel> RemoveItemFromCartAsync(int productId,Guid cartId)
        {
            var cart = await _cartRepository.GetByIdAsync(cartId)
                ?? throw new CartNotFoundException();

            var item = cart.Products.FirstOrDefault(p => p.Id == productId)
                ?? throw new ProductNotFoundException();

            cart.Products.Remove(item);

            await _cartRepository.UpdateAsync(cart);
            return cart;
        }

        public async Task<CartModel> CleanCartAsync(int productId,Guid cartId)
        {
            var cart = await _cartRepository.GetByIdAsync(cartId)
                ?? throw new CartNotFoundException();
            cart.Products.Clear();
            await _cartRepository.UpdateAsync(cart);
            return cart;
        }

    }

    public interface ICartService
    {
        public Task<CartModel> CalculateCartAsync(Guid cartId);
        public Task<CartModel> AddItemToCartAsync(int productId, Guid cartId);
        public Task<CartModel> RemoveItemFromCartAsync(int productId, Guid cartId);
        public Task<CartModel> CleanCartAsync(int productId, Guid cartId);
    }
}