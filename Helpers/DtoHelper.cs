using SRI02_Api.Models;

namespace SRI02_Api.Helpers
{
    public static class DtoHelper
    {
        public static Producent ConvertToEntity(ProducentDto producentDto)
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

        public static ProducentDto ConvertToDto(Producent producent)
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

        public static Product ConvertToEntity(ProductDto productDto)
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

        public static ProductDto ConvertToDto(Product product)
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
