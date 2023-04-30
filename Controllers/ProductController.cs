using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SRI02_Api.Helpers;
using SRI02_Api.Models;
using System.Data.Entity.Validation;

namespace SRI02_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly AppDbContext _db;
        private readonly LinkGenerator _linkGenerator;
        private readonly HttpContext _httpContext;

        public ProductController(ILogger<ProductController> logger, AppDbContext db, LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _db = db;
            _linkGenerator = linkGenerator;
            _httpContext = httpContextAccessor.HttpContext;
        }

        [HttpGet(Name = "GetProducts")]
        public IActionResult Get()
        {
            try 
            {
                var products = _db.Products.ToList();
                List<ProductDto> productsDto = new List<ProductDto>();
                foreach (var product in products)
                {
                    _db.Entry(product).Reference(x => x.Producent).Load();
                    var dto = DtoHelper.ConvertToDto(product);
                    dto.Links = GenerateLinks(product.Id);
                    productsDto.Add(dto);
                }

                ResponseEntity<List<ProductDto>> res = new ResponseEntity<List<ProductDto>>();
                res.ErrorMessage = null;
                res.StatusCode = System.Net.HttpStatusCode.OK;
                res.result = productsDto;

                return Ok(res);
            }
            catch (Exception e)
            {
                ResponseEntity<Product> res = new ResponseEntity<Product>();
                res.ErrorMessage = e.Message;
                res.StatusCode = System.Net.HttpStatusCode.BadRequest;
                res.result = null;
                return BadRequest(res);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var product = _db.Products.FirstOrDefault(x => x.Id == id);
                if (product != null)
                {
                    _db.Entry(product).Reference(x => x.Producent).Load();
                    var dto = DtoHelper.ConvertToDto(product);
                    dto.Links = GenerateLinks(product.Id);

                    ResponseEntity<ProductDto> res = new ResponseEntity<ProductDto>();
                    res.ErrorMessage = null;
                    res.StatusCode = System.Net.HttpStatusCode.OK;
                    res.result = dto;
                    return Ok(res);
                }
                else
                {
                    ResponseEntity<ProductDto> res = new ResponseEntity<ProductDto>();
                    res.ErrorMessage = null;
                    res.StatusCode = System.Net.HttpStatusCode.NotFound;
                    res.result = null;
                    return NotFound(res);
                }
            }
            catch (Exception e)
            {
                ResponseEntity<Product> res = new ResponseEntity<Product>();
                res.ErrorMessage = e.Message;
                res.StatusCode = System.Net.HttpStatusCode.BadRequest;
                res.result = null;
                return BadRequest(res);
            }
        }

        [HttpPost(Name = "CreateProduct")]
        public IActionResult Post([FromBody] ProductDto product)
        {
            try
            {
                _db.Products.Add(DtoHelper.ConvertToEntity(product));
                _db.SaveChanges();

                return CreatedAtAction("Post", DtoHelper.ConvertToEntity(product));
            }
            catch (DbEntityValidationException ex)
            {
                var error = ex.EntityValidationErrors.First().ValidationErrors.First();
                ResponseEntity<Product> res = new ResponseEntity<Product>();
                res.ErrorMessage = $"{error.PropertyName}, {error.ErrorMessage}";
                res.StatusCode = System.Net.HttpStatusCode.BadRequest;
                res.result = null;
                return BadRequest(res);
            }
            catch (Exception e)
            {
                ResponseEntity<Product> res = new ResponseEntity<Product>();
                res.ErrorMessage = e.Message;
                res.StatusCode = System.Net.HttpStatusCode.BadRequest;
                res.result = null;
                return BadRequest(res);
            }
        }

        [HttpPut(Name = "UpdateProduct")]
        public IActionResult Update([FromBody] ProductDto productDto, [FromQuery] int id)
        {
            try
            {
                var product = _db.Products.FirstOrDefault(x => x.Id == id);
                if (product != null)
                {
                    _db.ChangeTracker.Clear();
                    productDto.Id = id;
                    product = DtoHelper.ConvertToEntity(productDto);
                    _db.Products.Update(product);
                    _db.SaveChanges();

                    return NoContent();
                }
                else
                {
                    ResponseEntity<ProductDto> res = new ResponseEntity<ProductDto>();
                    res.ErrorMessage = null;
                    res.StatusCode = System.Net.HttpStatusCode.NotFound;
                    res.result = null;
                    return NotFound(res);
                }
            }
            catch (DbEntityValidationException ex)
            {
                var error = ex.EntityValidationErrors.First().ValidationErrors.First();
                ResponseEntity<Product> res = new ResponseEntity<Product>();
                res.ErrorMessage = $"{error.PropertyName}, {error.ErrorMessage}";
                res.StatusCode = System.Net.HttpStatusCode.BadRequest;
                res.result = null;
                return BadRequest(res);
            }
            catch (Exception e)
            {
                ResponseEntity<Product> res = new ResponseEntity<Product>();
                res.ErrorMessage = e.Message;
                res.StatusCode = System.Net.HttpStatusCode.BadRequest;
                res.result = null;
                return BadRequest(res);
            }
        }

        [HttpDelete(Name = "DeleteProduct")]
        public IActionResult Delete([FromQuery] int id)
        {
            try 
            { 
                var product = _db.Products.FirstOrDefault(x => x.Id == id);

                if (product != null)
                {
                    _db.Products.Remove(product);
                    _db.SaveChanges();

                    ResponseEntity<ProductDto> res = new ResponseEntity<ProductDto>();
                    res.ErrorMessage = null;
                    res.StatusCode = System.Net.HttpStatusCode.OK;
                    res.result = null;
                    return Ok(res);
                }

                return NotFound();
            }
            catch (Exception e)
            {
                ResponseEntity<Product> res = new ResponseEntity<Product>();
                res.ErrorMessage = e.Message;
                res.StatusCode = System.Net.HttpStatusCode.BadRequest;
                res.result = null;
                return BadRequest(res);
            }
        }

        [HttpGet("{id}/Producent")]
        public IActionResult GetProductDetails(int id)
        {
            try
            {
                var product = _db.Products.FirstOrDefault(x => x.Id == id);
                if (product != null)
                {
                    _db.Entry(product).Reference(x => x.Producent).Load();
                    if (product.Producent == null)
                    {
                        ResponseEntity<ProducentDto> res = new ResponseEntity<ProducentDto>();
                        res.ErrorMessage = null;
                        res.StatusCode = System.Net.HttpStatusCode.NotFound;
                        res.result = null;
                        return NotFound(res);
                    }
                    else
                    {
                        var dto = DtoHelper.ConvertToDto(product.Producent);
                        dto.Links = GenerateLinks(product.Producent.Id);

                        ResponseEntity<ProducentDto> res = new ResponseEntity<ProducentDto>();
                        res.ErrorMessage = null;
                        res.StatusCode = System.Net.HttpStatusCode.OK;
                        res.result = dto;
                        return Ok(res);
                    }
                }
                else
                {
                    ResponseEntity<ProductDto> res = new ResponseEntity<ProductDto>();
                    res.ErrorMessage = null;
                    res.StatusCode = System.Net.HttpStatusCode.NotFound;
                    res.result = null;
                    return NotFound(res);
                }
            }
            catch (Exception e)
            {
                ResponseEntity<Product> res = new ResponseEntity<Product>();
                res.ErrorMessage = e.Message;
                res.StatusCode = System.Net.HttpStatusCode.BadRequest;
                res.result = null;
                return BadRequest(res);
            }
        }

        [HttpPost("{id}/Producent/{producentId}")]
        public IActionResult AddProductDetails(int id, int producentId)
        {
            try 
            {
                var producent = _db.Producents.FirstOrDefault(x => x.Id == producentId);
                var product = _db.Products.FirstOrDefault(x => x.Id == id);
                if (product != null)
                {
                    if (producent == null)
                    {
                        ResponseEntity<ProductDto> res = new ResponseEntity<ProductDto>();
                        res.ErrorMessage = null;
                        res.StatusCode = System.Net.HttpStatusCode.NotFound;
                        res.result = null;
                        return NotFound(res);
                    }

                    _db.ChangeTracker.Clear();

                    product.ProducentId = producentId;
                    product.Producent = producent;
                    _db.Products.Update(product);
                    _db.SaveChanges();
                    return CreatedAtAction("PostProducentProduct", producent);
                }
                else
                {
                    ResponseEntity<ProductDto> res = new ResponseEntity<ProductDto>();
                    res.ErrorMessage = null;
                    res.StatusCode = System.Net.HttpStatusCode.NotFound;
                    res.result = null;
                    return NotFound(res);
                }
            }
            catch (Exception e)
            {
                ResponseEntity<Product> res = new ResponseEntity<Product>();
                res.ErrorMessage = e.Message;
                res.StatusCode = System.Net.HttpStatusCode.BadRequest;
                res.result = null;
                return BadRequest(res);
            }
        }

        [HttpDelete("{id}/Producent/{producentId}")]
        public IActionResult DeleteProductDetails(int id, int producentId)
        {
            try
            {
                var product = _db.Products.FirstOrDefault(x => x.Id == id);
                if (product != null)
                {
                    if (product.Producent == null || product.Producent.Id != producentId)
                    {
                        ResponseEntity<ProductDto> res = new ResponseEntity<ProductDto>();
                        res.ErrorMessage = null;
                        res.StatusCode = System.Net.HttpStatusCode.NotFound;
                        res.result = null;
                        return NotFound(res);
                    }

                    product.Producent = null;
                    product.ProducentId = null;
                    _db.Products.Update(product);
                    _db.SaveChanges();

                    ResponseEntity<ProductDto> res2 = new ResponseEntity<ProductDto>();
                    res2.ErrorMessage = null;
                    res2.StatusCode = System.Net.HttpStatusCode.OK;
                    res2.result = null;
                    return Ok(res2);
                }
                else
                {
                    ResponseEntity<ProductDto> res = new ResponseEntity<ProductDto>();
                    res.ErrorMessage = null;
                    res.StatusCode = System.Net.HttpStatusCode.NotFound;
                    res.result = null;
                    return NotFound(res);
                }
            }
            catch (Exception e)
            {
                ResponseEntity<Product> res = new ResponseEntity<Product>();
                res.ErrorMessage = e.Message;
                res.StatusCode = System.Net.HttpStatusCode.BadRequest;
                res.result = null;
                return BadRequest(res);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public List<Link> GenerateLinks(int id)
        {
            List<Link> links = new List<Link>();
            links.Add(GetSelfLink(id));
            links.Add(GetPostLink(id));
            links.Add(GetUpdateLink());
            links.Add(GetDeleteLink(id));
            links.Add(GetDetailsLink(id));
            return links;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public Link GetSelfLink(int id)
        {
            return new Link(_linkGenerator.GetUriByAction(_httpContext, "GetById", "Product"), "self", "GET");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public Link GetPostLink(int id)
        {
            return new Link(_linkGenerator.GetUriByAction(_httpContext, "Post", "Product") + $"/{id}", "create", "POST");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public Link GetUpdateLink()
        {
            return new Link(_linkGenerator.GetUriByAction(_httpContext, "Update", "Product"), "update", "PUT");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public Link GetDeleteLink(int id)
        {
            return new Link(_linkGenerator.GetUriByAction(_httpContext, "Delete", "Product") + $"/{id}", "delete", "DELETE");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public Link GetDetailsLink(int id)
        {
            return new Link(_linkGenerator.GetUriByAction(_httpContext, "GetProductDetails", "Product", new { id = 1 }) + $"/{id}", "self details", "GET");
        }


    }
}
