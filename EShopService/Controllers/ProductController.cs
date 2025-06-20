using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EShop.Domain.Models.Products;
using EShop.Application.Services;
using AutoMapper;
using EShop.Application.Dto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EShopService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        // GET: api/<ProductController>
        [HttpGet]
        //[Authorize]
        [Authorize(Roles = "Administrator,Employee,Client")]
        public async Task<IActionResult> Get()
        {
            var result = await _productService.GetAllAsync();
            if (result == null || !result.Any())
            {
                return NotFound(new { message = "No products found." });
            }
            List<ProductDto> productDtos = _mapper.Map<List<ProductDto>>(result);
            return Ok(productDtos);
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator,Employee,Client")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _productService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound(new { message = $"Product with ID {id} not found." });
            }

            ProductDto productDto = _mapper.Map<ProductDto>(result);
            return Ok(productDto);
        }

        // POST api/<ProductController>
        [HttpPost]
        [Authorize(Roles = "Administrator,Employee")]
        public async Task<IActionResult> Post([FromBody] ProductDto productDto)
        {
            if (productDto == null)
            {
                throw new ArgumentNullException(nameof(productDto), "Product cannot be null.");
            }
            Product product = _mapper.Map<Product>(productDto);
            var result = await _productService.AddAsync(product);
            if (result == null)
            {
                throw new Exception("Product could not be added.");
            }
            return Ok(result);
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator,Employee")]
        public async Task<IActionResult> Put(int id, [FromBody] ProductDto productDto)
        {
            if (productDto == null)
            {
                throw new ArgumentNullException(nameof(productDto), "Product cannot be null.");
            }
            Product product = _mapper.Map<Product>(productDto);
            var result = await _productService.UpdateAsync(product);
            if (result == null)
            {
                throw new Exception("Product could not be added.");
            }
            return Ok(result);
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator,Employee")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
