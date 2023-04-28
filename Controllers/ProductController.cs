using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRI02_Api.Models;

namespace SRI02_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly AppDbContext _db;

        public ProductController(ILogger<ProductController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet(Name = "GetProducts")]
        public IActionResult Get()
        {
            var products = _db.Products.ToList();

            List<ProductDto> productsDto = new List<ProductDto>();
            foreach (var product in products)
            {
                productsDto.Add(ConvertToDto(product));
            }

            return Ok(productsDto);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _db.Products.FirstOrDefault(x => x.Id == id);
            if (product != null)
            {
                return Ok(ConvertToDto(product));
            }
            else
            {
                return NotFound();
            }

        }

        [HttpPost(Name = "CreateProduct")]
        public IActionResult Post([FromBody] ProductDto product)
        {
            _db.Products.Add(ConvertToEntity(product));
            _db.SaveChanges();

            return CreatedAtAction("Post", ConvertToEntity(product));
        }

        [HttpPut(Name = "UpdateProduct")]
        public IActionResult Update([FromBody] ProductDto productDto, [FromQuery] int id)
        {
            var product = _db.Products.FirstOrDefault(x => x.Id == id);
            if (product != null)
            {
                _db.ChangeTracker.Clear();
                productDto.Id = id;
                product = ConvertToEntity(productDto);
                _db.Products.Update(product);
                _db.SaveChanges();

                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete(Name = "DeleteProduct")]
        public IActionResult Delete([FromQuery] int id)
        {

            var product = _db.Products.FirstOrDefault(x => x.Id == id);

            if (product != null)
            {
                _db.Products.Remove(product);
                _db.SaveChanges();

                return Ok();
            }

            return NotFound();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public Product ConvertToEntity(ProductDto productDto)
        {
            Product product = new Product
            {
                Id = productDto.Id,
                Code = productDto.Code,
                Name = productDto.Name,
                Ean = productDto.Ean,
                Price = productDto.Price,
                Description = productDto.Description,
                IsAvailable = productDto.IsAvailable,
                ProducentId = productDto.ProducentId,
                Producent = productDto.Producent
            };

            return product;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public ProductDto ConvertToDto(Product product)
        {
            ProductDto dto = new ProductDto
            {
                Code = product.Code,
                Name = product.Name,
                Ean = product.Ean,
                Price = product.Price,
                Description = product.Description,
                IsAvailable = product.IsAvailable,
                ProducentId = product.ProducentId,
                Producent = product.Producent
            };

            return dto;
        }
    }
}
