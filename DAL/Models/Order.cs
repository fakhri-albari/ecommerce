using Nexus.Base.CosmosDBRepository;
using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public class OrderPaymentDetailStatus
    {
        public DateTime? created { get; set; }
        public DateTime? paid { get; set; }
        public DateTime? canceled { get; set; }
    }

    public class OrderPayment
    {
        public string id { get; set; }
        public string status { get; set; }
        public OrderPaymentDetailStatus detailStatus { get; set; }
        public string method { get; set; }
        public double total { get; set; }
    }

    public class OrderProduct
    {
        public string id { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
        public double total { get; set; }
    }

    public class OrderDetailStatus
    {
        public DateTime? waitForSeller { get; set; }
        public DateTime? processed { get; set; }
        public DateTime? delivered { get; set; }
        public DateTime? arrived { get; set; }
        public DateTime? finished { get; set; }
        public DateTime? rejected { get; set; }
        
    }

    public class OrderStore
    {
        public string storeId { get; set; }
        public IEnumerable<OrderProduct> products { get; set; }
        public string status { get; set; }
        public OrderDetailStatus detailStatus { get; set; }
    }

    public class Order : ModelBase
    {
        public string address { get; set; }
        public string contact { get; set; }
        public string date { get; set; }
        public string buyerId { get; set; }
        public OrderPayment payment { get; set; }
        public IEnumerable<OrderStore> stores { get; set; }
    }
}
