namespace ECommerceApp.Client.Services.Abstractions
{
    public interface ICategoryService
    {
        List<Category> Categories { get; set; }
        Task GetCategories();
    }
}
