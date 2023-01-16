using ECommerceApp.Shared.ViewModels;

namespace ECommerceApp.Client.Services.Abstractions
{
    public interface IProductService
    {
        List<Product> Products { get; set; }
        Task GetProducts();
        Task<ServiceResponse<Product>> GetProductById(int productId);
    }
}
