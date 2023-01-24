using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ECommerceApp.Client.Shared
{
    public partial class Search
    {
        private string _searchText = string.Empty;
        private List<string> _searchSuggestions = new List<string>();
        protected ElementReference searchInput;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
                await searchInput.FocusAsync();
        }

        public void SearchProducts()
        {
            NavigationManager.NavigateTo($"/search/{_searchText}");
        }

        public async Task HandleSearch(KeyboardEventArgs eventArgs)
        {
            if (eventArgs.Key == null || eventArgs.Key.Equals("Enter"))
                SearchProducts();
            else if (_searchText.Length > 1)
                _searchSuggestions = await ProductService.GetProductSearchSuggestions(_searchText);
        }
    }
}
