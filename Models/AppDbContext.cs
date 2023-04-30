using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace SRI02_Api.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            var created = this.Database.EnsureCreated();

            if (created)
            {
                this.Products.Add(new Product { Code = "SRI-3252", Name = "Samsung QLED TV", Ean = "123125456234", Price = 1000.55, Description = "Brand new Samsung TV", IsAvailable = true});
                this.Products.Add(new Product { Code = "SRI-999888", Name = "Bosh dishwasher", Ean = "527456342345", Price = 505.30, Description = "Brand new Bosh dishwasher", IsAvailable = true });
                this.Producents.Add(new Producent { Name = "Samsung", Nationtality = "USA", NIP = "938217329", PhoneNumber = "999888777"});
                this.SaveChanges();
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne<Producent>(s => s.Producent)
                .WithMany(g => g.Products)
                .HasForeignKey(s => s.ProducentId);

            modelBuilder.Entity<Producent>()
                .HasMany<Product>(s => s.Products);

            modelBuilder.Entity<Product>()
                .Property(s => s.Price).HasDefaultValue(0);

            modelBuilder.Entity<Product>()
                .Property(s => s.Code).HasMaxLength(128);

            modelBuilder.Entity<Product>()
                .Property(s => s.Name).HasMaxLength(256);

            modelBuilder.Entity<Product>()
                .Property(s => s.Description).HasMaxLength(256);

            modelBuilder.Entity<Producent>()
                .Property(s => s.Name).HasMaxLength(256);

            modelBuilder.Entity<Producent>()
                .Property(s => s.NIP).HasMaxLength(15);
        }

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Producent> Producents { get; set; } = null!;
    }
}
