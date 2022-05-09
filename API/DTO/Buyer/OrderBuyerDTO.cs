using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.DTO.Buyer
{
    public class OrderBuyerDTO
    {
        public string id { get; set; }
        public string orderId { get; set; }
        public string address { get; set; }
        public string contact { get; set; }
        public string date { get; set; }
        public string buyerId { get; set; }
        public string storeId { get; set; }
        public IEnumerable<OrderProduct> products { get; set; }
        public string status { get; set; }
        public OrderDetailStatus orderSellerDetailStatus { get; set; }
    }
}
