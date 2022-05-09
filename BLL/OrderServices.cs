using DAL.Models;
using Nexus.Base.CosmosDBRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public class OrderServices
    {
        private readonly IDocumentDBRepository<Order> _repo;

        public OrderServices(IDocumentDBRepository<Order> repo)
        {
            _repo = repo;
        }

        public async Task<Order> CreateOrder(Order order)
        {
            var result = await _repo.CreateAsync(order);
            return result;
        }

        public async Task<Order> UpdateOrderPayment(OrderPayment orderPayment, string orderId)
        {
            Order order = await _repo.GetByIdAsync(orderId);
            order.payment = orderPayment;
            await _repo.UpdateAsync(orderId, order);
            return order;
        }
    }
}
