using Contracts.Common;
using Microsoft.AspNetCore.Mvc;
using Product.API.Entities;
using Product.API.Persistence;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRepositoryBaseAsync<CatalogProduct, long, ProductDbContext> _repository;

        public ProductsController(IRepositoryBaseAsync<CatalogProduct, long, ProductDbContext> repository)
        {
            _repository = repository;
        }
        // GET: api/<ProductsController>
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var result = 0;
            return Ok(result);
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProductsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
