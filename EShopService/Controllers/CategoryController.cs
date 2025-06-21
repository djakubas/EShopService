using AutoMapper;
using EShop.Application.Dto;
using EShop.Application.DTO;
using EShop.Application.Services;
using EShop.Domain.Models.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShopService.Controllers
{
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        //[Authorize]
        //[Authorize(Roles = "Administrator,Employee,Client")]
        public async Task<IActionResult> Get()
        {
            var result = await _categoryService.GetAllAsync();
            if (result == null || !result.Any())
            {
                return NotFound(new { message = "No products found." });
            }
            List<CategoryDto> categoryDtos = _mapper.Map<List<CategoryDto>>(result);
            return Ok(categoryDtos);
        }
        
        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        //[Authorize(Roles = "Administrator,Employee,Client")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound(new { message = $"Category with ID {id} not found." });
            }

            CategoryDto categoryDto = _mapper.Map<CategoryDto>(result);
            return Ok(categoryDto);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Employee")]
        public async Task<IActionResult> Post([FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                throw new ArgumentNullException(nameof(categoryDto), "Product cannot be null.");
            }
            Category category = _mapper.Map<Category>(categoryDto);
            var result = await _categoryService.AddAsync(category);
            if (result == null)
            {
                throw new Exception("Category could not be added.");
            }
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator,Employee")]
        public async Task<IActionResult> Put(Guid id, [FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                throw new ArgumentNullException(nameof(categoryDto), "Product cannot be null.");
            }
            Category category = _mapper.Map<Category>(categoryDto);
            var result = await _categoryService.UpdateAsync(category);
            if (result == null)
            {
                throw new Exception("Product could not be added.");
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator,Employee")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _categoryService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message } );
            }

            
        }
    }
}
