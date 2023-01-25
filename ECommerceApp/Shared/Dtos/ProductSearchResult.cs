using ECommerceApp.Shared.Models;

namespace ECommerceApp.Shared.Dtos
{
    public class ProductSearchResult
    {
        public List<Product> Products { get; set; }
        public int Pages { get; set; }
        public int CurrenPage { get; set; }
    }
}
