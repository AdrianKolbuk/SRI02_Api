namespace SRI02_Api.Models
{
    public class ProducentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NIP { get; set; }
        public string PhoneNumber { get; set; }
        public string Nationtality { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
