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

    public class OrderStoreBase : ModelBase
    {
        public string orderId { get; set; }
        public string address { get; set; }
        public string contact { get; set; }
        public string date { get; set; }
        public string buyerId { get; set; }
        public string storeId { get; set; }
        public IEnumerable<OrderProduct> products { get; set; }
        public string status { get; set; }
        public OrderDetailStatus orderDetailStatus { get; set; }
    }
}
