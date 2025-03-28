using Microsoft.AspNetCore.Mvc;
using EShopService.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EShopService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        // GET: api/<ProductController>
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return Enumerable.Range(1, 5).Select(id => new Product
            {
                Category = new Category { Name = $"Kategoria {id}" },
                Id = id,
                UpdatedBy = Guid.NewGuid()
            }).ToArray();
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public Product Get(int id)
        {
            return new Product
            {
                Category = new Category { Name = "Kategoria" },
                Id = id,
                CreatedAt = DateTime.Now,
                CreatedBy = Guid.NewGuid()
            };
        }

        // POST api/<ProductController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
