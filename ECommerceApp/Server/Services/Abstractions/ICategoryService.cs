namespace ECommerceApp.Server.Services.Abstractions
{
    public interface ICategoryService
    {
        Task<ServiceResponse<List<Category>>> GetCategories();
    }
}
