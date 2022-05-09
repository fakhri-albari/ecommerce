using System;
using System.Collections.Generic;
using System.Text;

namespace API.DTO.Order
{
    public class OrderPaymentDTO
    {
        public string method { get; set; }
        public double total { get; set; }
    }

    public class OrderProductDTO
    {
        public string id { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
        public double total { get; set; }
    }

    public class OrderStoreDTO
    {
        public string storeId { get; set; }
        public IEnumerable<OrderProductDTO> products { get; set; }
    }

    public class OrderDTO
    {
        public string id { get; set; }
        public string address { get; set; }
        public string contact { get; set; }
        public string date { get; set; }
        public string buyerId { get; set; }
        public OrderPaymentDTO payment { get; set; }
        public IEnumerable<OrderStoreDTO> stores { get; set; }
    }
}
