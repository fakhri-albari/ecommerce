using DAL.Models;
using Nexus.Base.CosmosDBRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public class OrderBuyerServices
    {
        private readonly IDocumentDBRepository<OrderBuyerBase> _repo;

        public OrderBuyerServices(IDocumentDBRepository<OrderBuyerBase> repo)
        {
            _repo = repo;
        }

        public async Task<OrderBuyerBase> CreateOrderBuyer(OrderBuyerBase orderStore)
        {
            var result = await _repo.CreateAsync(orderStore);
            return result;
        }

        public async Task<OrderBuyerBase> UpdateOrderStatus(UpdateOrderStatus orderStatus)
        {
            string orderBuyerId = orderStatus.id + orderStatus.storeId;
            OrderBuyerBase order = await _repo.GetByIdAsync(orderBuyerId);
            order.status = orderStatus.status;
            order.orderDetailStatus = orderStatus.detailStatus;
            await _repo.UpdateAsync(orderBuyerId, order);
            return order;
        }
    }
}
