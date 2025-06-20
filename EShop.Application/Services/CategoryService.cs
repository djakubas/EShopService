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
    public class CategoryService : ICategoryService
    {
        //any business logic can be implemented in this service (e.g. add only if does not exist, update only if active etc.) 
        protected readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository repository)
        {
            _categoryRepository = repository;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }
        public async Task<Category?> GetByIdAsync(Guid id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }
        public async Task<Category> AddAsync(Category category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));
            return await _categoryRepository.AddAsync(category);
        }
        public async Task<Category> UpdateAsync(Category category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));
            return await _categoryRepository.UpdateAsync(category);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _categoryRepository.DeleteAsync(id);
        }
    }

    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(Guid id);
        Task<Category> AddAsync(Category category);
        Task<Category> UpdateAsync(Category category);
        Task DeleteAsync(Guid id);
    }
}
