using Product.API.Entities;
using ILogger = Serilog.ILogger;
namespace Product.API.Persistence
{
    public class ProductContextSeed
    {
        public static async Task SeedProductAsync(ProductDbContext context, ILogger logger)
        {
            if (!context.Products.Any())
            {
                context.AddRange(GetCatalogProducts());
                await context.SaveChangesAsync();
                logger.Information("Seed database associated with context {DbContextName}", typeof(ProductDbContext).Name);
            }
        }

        private static IEnumerable<CatalogProduct> GetCatalogProducts()
        {
            return new List<CatalogProduct>()
            {
                new()
                {
                    No = "P0001",
                    Name = "Product 1",
                    Summary = "Product 1 Summary",
                    Description = "Product 1 Description",
                    Price = (decimal)100.12
                },
                new()
                {
                    No = "P0002",
                    Name = "Product 2",
                    Summary = "Product 2 Summary",
                    Description = "Product 2 Description",
                    Price = (decimal)100.12
                },
            };
        }
    }
}
