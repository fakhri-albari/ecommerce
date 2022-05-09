using Nexus.Base.CosmosDBRepository;
using System;

namespace DAL.Models
{
    public class PaymentDetailStatus
    {
        public DateTime created { get; set; }
        public DateTime? paid { get; set; }
        public DateTime? canceled { get; set; }
    }
    public class Payment : ModelBase
    {
        public string status { get; set; }
        public PaymentDetailStatus detailStatus { get; set; }
        public string method { get; set; }
        public double total { get; set; }
        public string orderId { get; set; }
    }
}
