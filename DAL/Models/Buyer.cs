using Nexus.Base.CosmosDBRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class Buyer : ModelBase
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public DateTime birthOfDate { get; set; }
        public string registrationDate { get; set; }
    }

    public class OrderBuyerBase : ModelBase
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
