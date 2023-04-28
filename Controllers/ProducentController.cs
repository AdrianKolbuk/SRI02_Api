using Microsoft.AspNetCore.Mvc;
using SRI02_Api.Models;

namespace SRI02_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProducentController : ControllerBase
    {
        private readonly ILogger<ProducentController> _logger;
        private readonly AppDbContext _db;

        public ProducentController(ILogger<ProducentController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet(Name = "GetProducents")]
        public IActionResult Get()
        {
            var producents = _db.Producents.ToList();

            List<ProducentDto> producentsDto = new List<ProducentDto>();
            foreach (var producent in producents)
            {
                producentsDto.Add(ConvertToDto(producent));
            }

            return Ok(producentsDto);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var producent = _db.Producents.FirstOrDefault(x => x.Id == id);
            if (producent != null)
            {
                return Ok(ConvertToDto(producent));
            }
            else
            {
                return NotFound();
            }

        }

        [HttpPost(Name = "CreateProducent")]
        public IActionResult Post([FromBody] ProducentDto producent)
        {
            _db.Producents.Add(ConvertToEntity(producent));
            _db.SaveChanges();

            return CreatedAtAction("Post", ConvertToEntity(producent));
        }

        [HttpPut(Name = "UpdateProducent")]
        public IActionResult Update([FromBody] ProducentDto producentDto, [FromQuery] int id)
        {
            var producent = _db.Producents.FirstOrDefault(x => x.Id == id);
            if (producent != null)
            {
                _db.ChangeTracker.Clear();
                producentDto.Id = id;
                producent = ConvertToEntity(producentDto);
                _db.Producents.Update(producent);
                _db.SaveChanges();

                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete(Name = "DeleteProducent")]
        public IActionResult Delete([FromQuery] int id)
        {

            var producent = _db.Producents.FirstOrDefault(x => x.Id == id);

            if (producent != null)
            {
                _db.Producents.Remove(producent);
                _db.SaveChanges();

                return Ok();
            }

            return NotFound();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public Producent ConvertToEntity(ProducentDto producentDto)
        {
            Producent producent = new Producent
            {
                Id = producentDto.Id,
                Name = producentDto.Name,
                Nationtality = producentDto.Nationtality,
                NIP = producentDto.NIP,
                PhoneNumber = producentDto.PhoneNumber,
                Products = producentDto.Products
            };

            return producent;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public ProducentDto ConvertToDto(Producent producent)
        {
            ProducentDto dto = new ProducentDto
            {
                Name = producent.Name,
                Nationtality = producent.Nationtality,
                NIP = producent.NIP,
                PhoneNumber = producent.PhoneNumber,
                Products = producent.Products
            };

            return dto;
        }
    }
}
