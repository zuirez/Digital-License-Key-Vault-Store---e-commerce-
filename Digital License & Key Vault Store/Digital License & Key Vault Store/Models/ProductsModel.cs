namespace Digital_License___Key_Vault_Store.Models
{
    public class ProductsModel
    {
        public int ProductId { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int AvailableKeys { get; set; }
    }
}