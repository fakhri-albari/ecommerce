using DAL.Models;
using Nexus.Base.CosmosDBRepository;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace BLL
{
    public class SellerServices
    {
        private readonly IDocumentDBRepository<Seller> _repo;

        public SellerServices(IDocumentDBRepository<Seller> repo)
        {
            _repo = repo;
        }

        public async Task<Seller> CreateSeller(Seller seller)
        {
            var result = await _repo.CreateAsync(seller);
            return result;
        }

        public async Task<Seller> UpdateSeller(Seller seller)
        {
            var oldSeller = await _repo.GetByIdAsync(seller.Id, partitionKeys: new Dictionary<string, string>() { { "storeId", seller.storeId } });
            oldSeller.name = seller.name;
            oldSeller.password = seller.password;
            oldSeller.email = seller.email;
            var updatedSeller = await _repo.UpdateAsync(seller.Id, oldSeller);
            return updatedSeller;
        }

        public async Task<Store> UpdateStore(Seller seller)
        {
            var allOldSeler = await _repo.GetAsync(partitionKeys: new Dictionary<string, string>() { { "storeId", seller.storeId } });
            foreach(var oldSeller in allOldSeler.Items)
            {
                oldSeller.store = seller.store;
                await _repo.UpdateAsync(oldSeller.Id, oldSeller);
            }
            return seller.store;
        }
    }
}
