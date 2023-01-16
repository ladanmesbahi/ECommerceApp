namespace ECommerceApp.Client.Shared
{
    public partial class ProductList
    {
        protected override async Task OnInitializedAsync()
        {
            await ProductService.GetProducts();
        }
    }
}
