using API.DTO.Seller;

namespace API.DTO.Product
{
    public class ProductDTO
    {
        public string name { get; set; }
        public int price { get; set; }
        public int stock { get; set; }
        public string storeId { get; set; }
        public StoreDTO store { get; set; }
    }
}
