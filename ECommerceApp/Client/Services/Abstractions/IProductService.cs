using ECommerceApp.Shared.ViewModels;

namespace ECommerceApp.Client.Services.Abstractions
{
    public interface IProductService
    {
        event Action ProductsChanged;
        List<Product> Products { get; set; }
        Task GetProducts(string? categoryUrl = null);
        Task<ServiceResponse<Product>> GetProductById(int productId);
    }
}
