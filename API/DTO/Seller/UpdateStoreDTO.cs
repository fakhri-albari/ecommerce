using System;
using System.Collections.Generic;
using System.Text;

namespace API.DTO.Seller
{
    public class UpdateStoreDTO
    {
        public string storeId { get; set; }
        public StoreDTO store { get; set; }
    }
}
