using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.DTO.Order
{
    public class UpdateOrderPaymentDTO
    {
        public string id { get; set; }
        public string status { get; set; }
        public PaymentDetailStatus detailStatus { get; set; }
        public string method { get; set; }
        public double total { get; set; }
        public string orderId { get; set; }
    }
}
