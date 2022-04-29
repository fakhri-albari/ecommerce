using Newtonsoft.Json;

namespace DAL.Models
{
    public class Store
    {

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("information")]
        public string information { get; set; }
    }
}
