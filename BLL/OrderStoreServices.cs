using DAL.Models;
using Nexus.Base.CosmosDBRepository;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace BLL
{
    public class OrderStoreServices
    {
        private readonly IDocumentDBRepository<OrderStoreBase> _repo;

        public OrderStoreServices(IDocumentDBRepository<OrderStoreBase> repo)
        {
            _repo = repo;
        }

        public async Task<OrderStoreBase> CreateOrderStore(OrderStoreBase orderStore)
        {
            var result = await _repo.CreateAsync(orderStore);
            return result;
        }
    }
}
