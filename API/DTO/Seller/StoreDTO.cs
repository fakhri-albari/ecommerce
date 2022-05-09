using DAL.Models;
using System;
using System.Collections.Generic;

namespace API.DTO.Seller
{
    public class StoreDTO
    {
        public string name { get; set; }
        public string information { get; set; }
    }

    public class OrderStoreDetailStatusDTO
    {
        public DateTime? waitForSeller { get; set; }
        public DateTime? processed { get; set; }
        public DateTime? delivered { get; set; }
        public DateTime? arrived { get; set; }
        public DateTime? finished { get; set; }
        public DateTime? rejected { get; set; }
    }

    public class OrderStoreDTO
    {
        public string orderId { get; set; }
        public string address { get; set; }
        public string contact { get; set; }
        public string date { get; set; }
        public string buyerId { get; set; }
        public string storeId { get; set; }
        public IEnumerable<OrderStoreProduct> products { get; set; }
        public string status { get; set; }
        public OrderStoreDetailStatusDTO orderSellerDetailStatus { get; set; }
    }
}
