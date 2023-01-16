using ECommerceApp.Shared.ViewModels;
using System.Net.Http.Json;

namespace ECommerceApp.Client.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public List<Product> Products { get; set; }
        public async Task GetProducts()
        {
            var response = await _httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product");
            if (response is { Success: true, Data: { } })
                Products = response.Data;
        }

        public async Task<ServiceResponse<Product>> GetProductById(int productId)
        {
            return await _httpClient.GetFromJsonAsync<ServiceResponse<Product>>
            ($"api/product/{productId}");
        }
    }
}
