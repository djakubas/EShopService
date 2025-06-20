using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Domain.Exceptions.Products;
using EShop.Domain.Models.Products;
using EShop.Domain.Repositories;
using Microsoft.Identity.Client;

namespace EShop.Application.Services
{
    public class ProductService : IProductService
    {
        //any business logic can be implemented in this service (e.g. add only if does not exist, update only if active etc.) 
        protected readonly IProductRepository _productRepository;
        public ProductService(IProductRepository repository)
        {
            _productRepository = repository;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _productRepository.GetAllAsync();
        }
        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }
        public async Task<Product> AddAsync(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            return await _productRepository.AddAsync(product);
        }
        public async Task<Product> UpdateAsync(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            return await _productRepository.UpdateAsync(product);
        }
        public async Task<Product> DeleteAsync(int id)
        {
            var p = await _productRepository.GetByIdAsync(id)
                ?? throw new ProductNotFoundException($"Product with id {id} not found.");
            p.Deleted = true;
            return await _productRepository.UpdateAsync(p);
        }
    }

    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product> AddAsync(Product product);
        Task<Product> UpdateAsync(Product product);
        Task<Product> DeleteAsync(int id);
    }
}
