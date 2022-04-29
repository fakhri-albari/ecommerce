using System;
using System.Collections.Generic;
using System.Text;

namespace API.DTO.Product
{
    public class UpdateProductDTO
    {
        public string id { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public int stock { get; set; }
        public string storeId { get; set; }
    }
}
