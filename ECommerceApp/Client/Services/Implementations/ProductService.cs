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

        public event Action ProductsChanged;
        public List<Product> Products { get; set; } = new List<Product>();
        public string Message { get; set; } = "Loading products... ";

        public async Task GetProducts(string? categoryUrl = null)
        {
            var response = categoryUrl is null ? await _httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product") :
                await _httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/category/{categoryUrl}");
            if (response is { Success: true, Data: { } })
                Products = response.Data;
            ProductsChanged.Invoke();
        }
        public async Task<ServiceResponse<Product>> GetProductById(int productId)
        {
            return await _httpClient.GetFromJsonAsync<ServiceResponse<Product>>
            ($"api/product/{productId}");
        }

        public async Task SearchProducts(string searchText)
        {
            var response = await _httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/Product/search/{searchText}");
            if (response is { Success: true, Data: { } })
                Products = response.Data;
            if (Products.Count == 0)
                Message = "No product found!";
            ProductsChanged.Invoke();
        }

        public async Task<List<string>> GetProductSearchSuggestions(string searchText)
        {
            return (await _httpClient.GetFromJsonAsync<ServiceResponse<List<string>>>($"api/Product/searchSuggestions/{searchText}"))?.Data;
        }
    }
}
