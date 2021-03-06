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
        public async Task<OrderStoreBase> UpdateOrderStatus(UpdateOrderStatus orderStatus)
        {
            string orderBuyerId = orderStatus.id + orderStatus.storeId;
            OrderStoreBase order = await _repo.GetByIdAsync(orderBuyerId);
            order.status = orderStatus.status;
            order.orderDetailStatus = orderStatus.detailStatus;
            await _repo.UpdateAsync(orderBuyerId, order);
            return order;
        }
    }
}
