namespace ECommerceApp.Shared.ViewModels
{
    public class CartProductResponse
    {
        public int ProductId { get; set; }
        public string Title { get; set; } = String.Empty;
        public int ProductTypeId { get; set; }
        public string ProductType { get; set; } = String.Empty;
        public string ImageUrl { get; set; } = String.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
