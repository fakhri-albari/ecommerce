using DAL.Models;
using Microsoft.Azure.Cosmos;
using Nexus.Base.CosmosDBRepository;
using System;

namespace DAL
{
    public class Repositories
    {
        private static readonly string _eventGridKey = Environment.GetEnvironmentVariable("eventGridKey");
        private static readonly string _eventGridEndPoint = Environment.GetEnvironmentVariable("eventGridEndpoint");

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

        public class OrderRepository : DocumentDBRepository<Order>
        {
            public OrderRepository(CosmosClient client) : base("Order", client, partitionProperties: "date", eventGridKey: _eventGridKey, eventGridEndPoint: _eventGridEndPoint)
            {

            }
        }

        public class PaymentRepository : DocumentDBRepository<Payment>
        {
            public PaymentRepository(CosmosClient client) : base("Order", client, partitionProperties: "method", eventGridKey: _eventGridKey, eventGridEndPoint: _eventGridEndPoint)
            {

            }
        }

        public class OrderStoreRepository : DocumentDBRepository<OrderStoreBase>
        {
            public OrderStoreRepository(CosmosClient client) : base("Seller", client, partitionProperties: "storeId", eventGridKey: _eventGridKey, eventGridEndPoint: _eventGridEndPoint)
            {

            }
        }
    }
}
