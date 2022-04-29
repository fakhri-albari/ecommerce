using Newtonsoft.Json;
using Nexus.Base.CosmosDBRepository;

namespace DAL.Models
{
    public class Product : ModelBase
    {
        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("price")]
        public int price { get; set; }

        [JsonProperty("stock")]
        public int stock { get; set; }

        [JsonProperty("storeId")]
        public string storeId { get; set; }

        [JsonProperty("store")]
        public Store store { get; set; }
    }
}
