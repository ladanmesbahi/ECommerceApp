namespace ECommerceApp.Client.Services.Abstractions
{
    public interface IProductService
    {
        List<Product> Products { get; set; }
        Task GetProducts();
    }
}
