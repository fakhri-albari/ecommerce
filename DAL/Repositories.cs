using DAL.Models;
using Microsoft.Azure.Cosmos;
using Nexus.Base.CosmosDBRepository;

namespace DAL
{
    public class Repositories
    {
        //private static readonly string _eventGridKey = Environment.GetEnvironmentVariable("eventGridKey");
        //private static readonly string _eventGridEndPoint = Environment.GetEnvironmentVariable("eventGridEndpoint");

        public class SellerRepository : DocumentDBRepository<Seller>
        {
            public SellerRepository(CosmosClient client) : base("Seller", client, partitionProperties: "storeId")
            {
            }
        }

        public class ProductRepository : DocumentDBRepository<Product>
        {
            public ProductRepository(CosmosClient client) : base("Product", client, partitionProperties: "storeId")
            {
            }
        }
    }
}
