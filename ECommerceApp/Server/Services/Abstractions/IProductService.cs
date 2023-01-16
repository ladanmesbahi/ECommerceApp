namespace ECommerceApp.Server.Services.Abstractions
{
    public interface IProductService
    {
        Task<ServiceResponse<List<Product>>> GetProducts();
        Task<object?> GetProductById(int id);
    }
}
