using DAL.Models;
using Nexus.Base.CosmosDBRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public class ProductServices
    {
        private readonly IDocumentDBRepository<Product> _repo;

        public ProductServices(IDocumentDBRepository<Product> repo)
        {
            _repo = repo;
        }

        public async Task<Product> CreateProduct(Product product)
        {
            var result = await _repo.CreateAsync(product);
            return result;
        }

        public async Task<PageResult<Product>> GetAllProduct()
        {
            var result = await _repo.GetAsync(predicate: p => p.ActiveFlag.Equals("Y")) ;
            return result;
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            var oldProduct = await _repo.GetByIdAsync(product.Id, partitionKeys: new Dictionary<string, string>() { { "storeId", product.storeId } });
            oldProduct.name = product.name;
            oldProduct.price = product.price;
            oldProduct.stock = product.stock;
            var updatedProduct = await _repo.UpdateAsync(product.Id, oldProduct);
            return updatedProduct;
        }

        public async Task DeleteProduct(Product product)
        {
            await _repo.DeleteAsync(product.Id, partitionKeys: new Dictionary<string, string>() { { "storeId", product.storeId } });
        }
    }
}
