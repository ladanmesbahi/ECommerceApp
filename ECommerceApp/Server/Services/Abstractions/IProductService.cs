namespace ECommerceApp.Server.Services.Abstractions
{
    public interface IProductService
    {
        Task<ServiceResponse<List<Product>>> GetProducts();
        Task<ServiceResponse<Product>> GetProductById(int id);
        Task<ServiceResponse<List<Product>>> GetProductsByCategory(string categoryUrl);
    }
}
