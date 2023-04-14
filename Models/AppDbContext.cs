using Microsoft.EntityFrameworkCore;

namespace SRI02_Api.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            var created = this.Database.EnsureCreated();

            if (created)
            {
                this.Products.Add(new Product { Code = "SRI-3252", Name = "Samsung QLED TV", Ean = "123125456234", Price = 1000.55, Description = "Brand new Samsung TV", IsAvailable = true });
                this.Products.Add(new Product { Code = "SRI-999888", Name = "Bosh dishwasher", Ean = "527456342345", Price = 505.30, Description = "Brand new Bosh dishwasher", IsAvailable = true });
                this.SaveChanges();
            }
        }

        public DbSet<Product> Products { get; set; } = null!;
    }
}
