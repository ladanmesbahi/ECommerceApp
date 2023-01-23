using Microsoft.AspNetCore.Components;

namespace ECommerceApp.Client.Pages
{
    public partial class Index
    {
        [Parameter]
        public string? CategoryUrl { get; set; } = null;

        protected override async Task OnParametersSetAsync()
        {
            await ProductService.GetProducts(CategoryUrl);
        }
    }
}
