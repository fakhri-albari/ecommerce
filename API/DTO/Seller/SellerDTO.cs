namespace API.DTO.Seller
{
    public class SellerDTO
    {
        public string name { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string storeId { get; set; }
        public StoreDTO store { get; set; }
    }
}
