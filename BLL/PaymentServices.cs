using DAL.Models;
using Nexus.Base.CosmosDBRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class PaymentServices
    {
        private readonly IDocumentDBRepository<Payment> _repo;

        public PaymentServices(IDocumentDBRepository<Payment> repo)
        {
            _repo = repo;
        }
        public async Task<Payment> CreatePayment(Payment payment)
        {
            payment.detailStatus = new PaymentDetailStatus
            {
                created = DateTime.Now
            };
            payment.status = "created";
            var result = await _repo.CreateAsync(payment);
            return result;
        }
        public async Task UpdatePayment(Payment payment)
        {
            payment.detailStatus = new PaymentDetailStatus
            {
                created = DateTime.Now
            };
            payment.status = "created";
            var result = await _repo.CreateAsync(payment);
        }
    }
}
