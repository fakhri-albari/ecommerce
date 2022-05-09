using System;
using System.Collections.Generic;
using System.Text;

namespace API.DTO.Buyer
{
    public class BuyerDTO
    {
        public string id  { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public DateTime birthOfDate { get; set; }
        public string registrationDate { get; set; }
    }
}
