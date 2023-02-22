using ECommerceApp.Shared.Dtos;
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
        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; } = 0;
        public string LastSearchText { get; set; } = string.Empty;
        public List<Product> AdminProducts { get; set; }

        public async Task GetProducts(string? categoryUrl = null)
        {
            var response = categoryUrl is null ? await _httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product/featured") :
                await _httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/category/{categoryUrl}");
            if (response is { Success: true, Data: { } })
                Products = response.Data;

            CurrentPage = 1;
            PageCount = 0;

            if (Products.Count == 0)
                Message = "No products found.";
            ProductsChanged.Invoke();
        }
        public async Task<ServiceResponse<Product>> GetProductById(int productId)
        {
            return await _httpClient.GetFromJsonAsync<ServiceResponse<Product>>
            ($"api/product/{productId}");
        }

        public async Task SearchProducts(string searchText, int page)
        {
            LastSearchText = searchText;
            var response = await _httpClient.GetFromJsonAsync<ServiceResponse<ProductSearchResult>>($"api/Product/search/{searchText}/{page}");
            if (response is { Success: true, Data: { } })
            {
                Products = response.Data.Products;
                CurrentPage = response.Data.CurrenPage;
                PageCount = response.Data.Pages;
            }
            if (Products.Count == 0)
                Message = "No product found!";
            ProductsChanged.Invoke();
        }

        public async Task<List<string>> GetProductSearchSuggestions(string searchText)
        {
            return (await _httpClient.GetFromJsonAsync<ServiceResponse<List<string>>>($"api/Product/searchSuggestions/{searchText}"))?.Data;
        }

        public async Task GetAdminProducts()
        {
            var response = await _httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product/admin");
            if (response is { Success: true, Data: { } })
                AdminProducts = response.Data;

            CurrentPage = 1;
            PageCount = 0;

            if (Products.Count == 0)
                Message = "No products found.";
        }

        public async Task<Product> CreateProduct(Product product)
        {
            var response = await _httpClient.PostAsJsonAsync("api/product/admin", product);
            return (await response.Content.ReadFromJsonAsync<ServiceResponse<Product>>()).Data;
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            var response = await _httpClient.PutAsJsonAsync("api/product/admin", product);
            return (await response.Content.ReadFromJsonAsync<ServiceResponse<Product>>()).Data;
        }

        public async Task DeleteProduct(int productId)
        {
            await _httpClient.DeleteAsync($"api/product/admin/{productId}");
        }
    }
}
