using DAL.Models;
using Nexus.Base.CosmosDBRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public class BuyerServices
    {
        private readonly IDocumentDBRepository<Buyer> _repo;

        public BuyerServices(IDocumentDBRepository<Buyer> repo)
        {
            _repo = repo;
        }

        public async Task<Buyer> CreateBuyer(Buyer buyer)
        {
            var result = await _repo.CreateAsync(buyer);
            return result;
        }
    }
}
