using Contracts.Common;
using Infrastructures.Common;
using Microsoft.EntityFrameworkCore;
using Product.API.Entities;
using Product.API.Persistence;

namespace Product.API.Repositories
{
    public class ProductRepository : RepositoryBaseAsync<CatalogProduct, long, ProductDbContext>, IProductRepository
    {

        public ProductRepository(ProductDbContext dbContext, IUnitOfWork<ProductDbContext> unitOfWork) : base(dbContext, unitOfWork)
        {
        }


        public async Task<IEnumerable<CatalogProduct>> GetProducts() => await FindAll().ToListAsync();

        public Task<CatalogProduct> GetProduct(long id) => GetByIdAsync(id);

        public Task<CatalogProduct> GetProductByNo(string productNo) => 
            FindAllByCondition(x => x.No.Equals(productNo)).SingleOrDefaultAsync();
        

        public Task CreateProduct(CatalogProduct product) => CreateAsync(product);

        public Task UpdateProduct(CatalogProduct product) => UpdateAsync(product);
        public async Task DeleteProduct(long id)
        {
            var product = await GetProduct(id);
            if (product != null)  DeleteAsync(product);
        }
    }
}
