using System.Net.Http.Json;

namespace ECommerceApp.Client.Shared
{
    public partial class ProductList
    {
        private static List<Product> _products = new List<Product>();

        protected override async Task OnInitializedAsync()
        {
            var result = await Http.GetFromJsonAsync<List<Product>>("api/product");
            if (result != null)
                _products = result;
        }
    }
}
