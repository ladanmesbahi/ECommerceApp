using ECommerceApp.Shared.ViewModels;
using System.Net.Http.Json;

namespace ECommerceApp.Client.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _http;

        public CategoryService(HttpClient http)
        {
            _http = http;
        }

        public List<Category> Categories { get; set; } = new List<Category>();
        public async Task GetCategories()
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<Category>>>("api/Category");
            if (response is { Data: { } })
                Categories = response.Data;
        }
    }
}
