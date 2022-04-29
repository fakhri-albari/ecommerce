using Newtonsoft.Json;
using Nexus.Base.CosmosDBRepository;

namespace DAL.Models
{
    public class Seller : ModelBase
    {
        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("password")]
        public string password { get; set; }

        [JsonProperty("email")]
        public string email { get; set; }

        [JsonProperty("storeId")]
        public string storeId { get; set; }

        [JsonProperty("store")]
        public Store store { get; set; }
    }
}
