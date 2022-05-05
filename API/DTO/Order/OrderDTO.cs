using System;
using System.Collections.Generic;
using System.Text;

namespace API.DTO.Order
{
    public class OrderPaymentDetailStatusDTO
    {
        public DateTime created { get; set; }
        public DateTime paid { get; set; }
        public DateTime canceled { get; set; }
    }

    public class OrderPaymentDTO
    {
        public string status { get; set; }
        public OrderPaymentDetailStatusDTO detailStatus { get; set; }
        public string method { get; set; }
        public string bank { get; set; }
    }

    public class OrderProduct
    {
        public string id { get; set; }
        public int quantity { get; set; }
    }

    public class OrderOrderProductDTO
    {
        public string storeId { get; set; }
        public string storePickupPoint { get; set; }
        public OrderProduct product { get; set; }
    }

    public class OrderDTO
    {
        public string address { get; set; }
        public string contact { get; set; }
        public string date { get; set; }
        public string buyerId { get; set; }
        public OrderPaymentDTO payment { get; set; }
        public IEnumerable<OrderOrderProductDTO> product { get; set; }
    }
}
