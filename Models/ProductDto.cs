using System.ComponentModel.DataAnnotations;

namespace SRI02_Api.Models
{
    public class ProductDto
    {
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public string Ean { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Value should be greater than or equal to 0")]
        public double Price { get; set; }
        public string Description { get; set; }
        [Required]
        public bool IsAvailable { get; set; }
        public int? ProducentId { get; set; }
        public Producent? Producent { get; set; }
        public List<Link> Links { get; set; }
    }
}
