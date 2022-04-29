using System;
using System.Collections.Generic;
using System.Text;

namespace API.DTO.Seller
{
    public class UpdateSellerDTO
    {
        public string id { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string storeId { get; set; }
    }
}
