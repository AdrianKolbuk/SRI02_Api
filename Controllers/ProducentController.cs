using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SRI02_Api.Helpers;
using SRI02_Api.Models;
using System.Data.Entity;
using System.Data.Entity.Validation;
using static Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions;

namespace SRI02_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProducentController : ControllerBase
    {
        private readonly ILogger<ProducentController> _logger;
        private readonly AppDbContext _db;
        private readonly LinkGenerator _linkGenerator;
        private readonly HttpContext _httpContext;

        public ProducentController(ILogger<ProducentController> logger, AppDbContext db, LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _db = db;
            _linkGenerator = linkGenerator;
            _httpContext = httpContextAccessor.HttpContext;
        }

        [HttpGet(Name = "GetProducents")]
        public IActionResult Get()
        {
            try 
            {
                var producents = _db.Producents;
                
                foreach (var producent in producents)
                {
                    _db.Entry(producent).Collection(x => x.Products).Load();
                }

                List<ProducentDto> producentsDto = new List<ProducentDto>();
                foreach (var producent in producents)
                {
                    var dto = DtoHelper.ConvertToDto(producent);
                    dto.Links = GenerateLinks(producent.Id);
                    producentsDto.Add(dto);
                }

                ResponseEntity<List<ProducentDto>> res = new ResponseEntity<List<ProducentDto>>();
                res.ErrorMessage = null;
                res.StatusCode = System.Net.HttpStatusCode.OK;
                res.result = producentsDto;

                return Ok(res);
            }
            catch (Exception e)
            {
                ResponseEntity<Producent> res = new ResponseEntity<Producent>();
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
                var producent = _db.Producents.FirstOrDefault(x => x.Id == id);

                if (producent != null)
                {
                    _db.Entry(producent).Collection(x => x.Products).Load();
                    var dto = DtoHelper.ConvertToDto(producent);
                    dto.Links = GenerateLinks(producent.Id);

                    ResponseEntity<ProducentDto> res = new ResponseEntity<ProducentDto>();
                    res.ErrorMessage = null;
                    res.StatusCode = System.Net.HttpStatusCode.OK;
                    res.result = dto;

                    return Ok(res);
                }
                else
                {
                    ResponseEntity<List<ProducentDto>> res = new ResponseEntity<List<ProducentDto>>();
                    res.ErrorMessage = null;
                    res.StatusCode = System.Net.HttpStatusCode.NotFound;
                    res.result = null;
                    return NotFound(res);
                }
            }
            catch (Exception e)
            {
                ResponseEntity<Producent> res = new ResponseEntity<Producent>();
                res.ErrorMessage = e.Message;
                res.StatusCode = System.Net.HttpStatusCode.BadRequest;
                res.result = null;
                return BadRequest(res);
            }

        }

        [HttpPost(Name = "CreateProducent")]
        public IActionResult Post([FromBody] ProducentDto producent)
        {
            try
            {
                _db.Producents.Add(DtoHelper.ConvertToEntity(producent));
                _db.SaveChanges();

                return CreatedAtAction("Post", DtoHelper.ConvertToEntity(producent));

            }
            catch (DbEntityValidationException ex)
            {
                var error = ex.EntityValidationErrors.First().ValidationErrors.First();
                ResponseEntity<Producent> res = new ResponseEntity<Producent>();
                res.ErrorMessage = $"{error.PropertyName}, {error.ErrorMessage}";
                res.StatusCode = System.Net.HttpStatusCode.BadRequest;
                res.result = null;
                return BadRequest(res);
            }
            catch(Exception e)
            {
                ResponseEntity<Producent> res = new ResponseEntity<Producent>();
                res.ErrorMessage = e.Message;
                res.StatusCode = System.Net.HttpStatusCode.BadRequest;
                res.result = null;
                return BadRequest(res);
            }
        }

        [HttpPut(Name = "UpdateProducent")]
        public IActionResult Update([FromBody] ProducentDto producentDto, [FromQuery] int id)
        {
            try
            {
                var producent = _db.Producents.FirstOrDefault(x => x.Id == id);
                if (producent != null)
                {
                    _db.ChangeTracker.Clear();
                    producentDto.Id = id;
                    producent = DtoHelper.ConvertToEntity(producentDto);
                    _db.Producents.Update(producent);
                    _db.SaveChanges();

                    return NoContent();
                }
                else
                {
                    ResponseEntity<Producent> res = new ResponseEntity<Producent>();
                    res.ErrorMessage = null;
                    res.StatusCode = System.Net.HttpStatusCode.NotFound;
                    res.result = null;

                    return NotFound(res);
                }
            }
            catch (DbEntityValidationException ex)
            {
                var error = ex.EntityValidationErrors.First().ValidationErrors.First();
                ResponseEntity<Producent> res = new ResponseEntity<Producent>();
                res.ErrorMessage = $"{error.PropertyName}, {error.ErrorMessage}";
                res.StatusCode = System.Net.HttpStatusCode.BadRequest;
                res.result = null;
                return BadRequest(res);
            }
            catch (Exception e)
            {
                ResponseEntity<Producent> res = new ResponseEntity<Producent>();
                res.ErrorMessage = e.Message;
                res.StatusCode = System.Net.HttpStatusCode.BadRequest;
                res.result = null;
                return BadRequest(res);
            }
        }

        [HttpDelete(Name = "DeleteProducent")]
        public IActionResult Delete([FromQuery] int id)
        {
            try 
            { 
                var producent = _db.Producents.FirstOrDefault(x => x.Id == id);

                if (producent != null)
                {
                    _db.Producents.Remove(producent);
                    _db.SaveChanges();

                    ResponseEntity<Producent> res1 = new ResponseEntity<Producent>();
                    res1.ErrorMessage = null;
                    res1.StatusCode = System.Net.HttpStatusCode.OK;
                    res1.result = null;

                    return Ok(res1);
                }

                ResponseEntity<Producent> res = new ResponseEntity<Producent>();
                res.ErrorMessage = null;
                res.StatusCode = System.Net.HttpStatusCode.NotFound;
                res.result = null;

                return NotFound(res);
            }
            catch (Exception e)
            {
                ResponseEntity<Producent> res = new ResponseEntity<Producent>();
                res.ErrorMessage = e.Message;
                res.StatusCode = System.Net.HttpStatusCode.BadRequest;
                res.result = null;
                return BadRequest(res);
            }
        }

        [HttpGet("{id}/Products")]
        public IActionResult GetProducentDetails(int id)
        {
            try
            {
                var producent = _db.Producents.FirstOrDefault(x => x.Id == id);

                if (producent != null)
                {
                    _db.Entry(producent).Collection(x => x.Products).Load();

                    if (producent.Products != null)
                    {
                        List<ProductDto> dtos = new List<ProductDto>();
                        foreach (var product in producent.Products)
                        {
                            var dto = DtoHelper.ConvertToDto(product);
                            dto.Links = GenerateLinks(product.Id);
                            dtos.Add(dto);
                        }

                        ResponseEntity<List<ProductDto>> res = new ResponseEntity<List<ProductDto>>();
                        res.ErrorMessage = null;
                        res.StatusCode = System.Net.HttpStatusCode.OK;
                        res.result = dtos;

                        return Ok(res);
                    }
                    else
                    {
                        ResponseEntity<List<ProductDto>> res = new ResponseEntity<List<ProductDto>>();
                        res.ErrorMessage = null;
                        res.StatusCode = System.Net.HttpStatusCode.NotFound;
                        res.result = null;

                        return NotFound(res);
                    }
                }
                else
                {
                    ResponseEntity<List<ProductDto>> res = new ResponseEntity<List<ProductDto>>();
                    res.ErrorMessage = null;
                    res.StatusCode = System.Net.HttpStatusCode.NotFound;
                    res.result = null;
                    return NotFound(res);
                }
            }
            catch (Exception e)
            {
                ResponseEntity<Producent> res = new ResponseEntity<Producent>();
                res.ErrorMessage = e.Message;
                res.StatusCode = System.Net.HttpStatusCode.BadRequest;
                res.result = null;
                return BadRequest(res);
            }
        }

        [HttpPost("{id}/Products/{productId}")]
        public IActionResult AddProducentDetails(int id, int productId)
        {
            try
            {
                var product = _db.Products.FirstOrDefault(x => x.Id == productId);
                var producent = _db.Producents.FirstOrDefault(x => x.Id == id);
                if (producent != null)
                {
                    if (product == null)
                    {
                        ResponseEntity<List<ProductDto>> res = new ResponseEntity<List<ProductDto>>();
                        res.ErrorMessage = null;
                        res.StatusCode = System.Net.HttpStatusCode.NotFound;
                        res.result = null;
                        return NotFound(res);
                    }

                    if (producent.Products == null)
                    {
                        _db.ChangeTracker.Clear();
                        product.Producent = producent;
                        product.ProducentId = id;
                        List<Product> products = new List<Product>();
                        products.Add(product);
                        producent.Products = products;
                    }
                    else
                        producent.Products.Add(product);

                    _db.Producents.Update(producent);
                    _db.Products.Update(product);
                    _db.SaveChanges();
                    return CreatedAtAction("AddProducentDetails", producent);
                }
                else
                {
                    ResponseEntity<List<ProductDto>> res = new ResponseEntity<List<ProductDto>>();
                    res.ErrorMessage = null;
                    res.StatusCode = System.Net.HttpStatusCode.NotFound;
                    res.result = null;
                    return NotFound(res);
                }
            }
            catch (Exception e)
            {
                ResponseEntity<Producent> res = new ResponseEntity<Producent>();
                res.ErrorMessage = e.Message;
                res.StatusCode = System.Net.HttpStatusCode.BadRequest;
                res.result = null;
                return BadRequest(res);
            }
        }

        [HttpDelete("{id}/Products/{productId}")]
        public IActionResult DeleteProducentDetails(int id, int productId)
        {
            try 
            {
                var producent = _db.Producents.FirstOrDefault(x => x.Id == id);
                if (producent != null)
                {
                    var product = producent.Products.FirstOrDefault(x => x.Id == productId);

                    if (product == null)
                    {
                        ResponseEntity<List<ProductDto>> res = new ResponseEntity<List<ProductDto>>();
                        res.ErrorMessage = null;
                        res.StatusCode = System.Net.HttpStatusCode.NotFound;
                        res.result = null;
                        return NotFound(res);
                    }

                    producent.Products.Remove(product);
                    _db.Producents.Update(producent);
                    _db.SaveChanges();

                    ResponseEntity<List<ProductDto>> res2 = new ResponseEntity<List<ProductDto>>();
                    res2.ErrorMessage = null;
                    res2.StatusCode = System.Net.HttpStatusCode.OK;
                    res2.result = null;
                    return Ok(res2);
                }
                else
                {
                    ResponseEntity<List<ProductDto>> res = new ResponseEntity<List<ProductDto>>();
                    res.ErrorMessage = null;
                    res.StatusCode = System.Net.HttpStatusCode.NotFound;
                    res.result = null;
                    return NotFound(res);
                }
            }
            catch (Exception e)
            {
                ResponseEntity<Producent> res = new ResponseEntity<Producent>();
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
            return new Link(_linkGenerator.GetUriByAction(_httpContext, "GetById", "Producent"), "self", "GET");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public Link GetPostLink(int id)
        {
            return new Link(_linkGenerator.GetUriByAction(_httpContext, "Post", "Producent") + $"/{id}", "create", "POST");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public Link GetUpdateLink()
        {
            return new Link(_linkGenerator.GetUriByAction(_httpContext, "Update" ,"Producent"), "update", "PUT");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public Link GetDeleteLink(int id)
        {
            return new Link(_linkGenerator.GetUriByAction(_httpContext, "Delete", "Producent") + $"/{id}", "delete", "DELETE");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public Link GetDetailsLink(int id)
        {
            return new Link(_linkGenerator.GetUriByAction(_httpContext, "GetProducentDetails", "Producent", new { id = 1 }) + $"/{id}", "self details", "GET");
        }

    }
}
