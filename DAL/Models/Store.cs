using Newtonsoft.Json;
using Nexus.Base.CosmosDBRepository;
using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public class Store
    {

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("information")]
        public string information { get; set; }
    }

    public class OrderStoreProduct
    {
        public string id { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
        public double total { get; set; }
    }

    public class OrderStoreDetailStatus
    {
        public DateTime? waitForSeller { get; set; }
        public DateTime? processed { get; set; }
        public DateTime? delivered { get; set; }
        public DateTime? arrived { get; set; }
        public DateTime? finished { get; set; }
        public DateTime? rejected { get; set; }

    }

    public class OrderStoreBase : ModelBase
    {
        public string orderId { get; set; }
        public string address { get; set; }
        public string contact { get; set; }
        public string date { get; set; }
        public string buyerId { get; set; }
        public string storeId { get; set; }
        public IEnumerable<OrderStoreProduct> products { get; set; }
        public string status { get; set; }
        public OrderStoreDetailStatus orderSellerDetailStatus { get; set; }
    }
}
