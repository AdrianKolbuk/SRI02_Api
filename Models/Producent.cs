using System.ComponentModel.DataAnnotations;

namespace SRI02_Api.Models
{
    public class Producent
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string NIP { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public string Nationtality { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
