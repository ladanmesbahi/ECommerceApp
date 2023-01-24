namespace ECommerceApp.Client.Shared
{
    public partial class FeaturedProducts : IDisposable
    {
        protected override void OnInitialized()
        {
            ProductService.ProductsChanged += StateHasChanged;
        }
        public void Dispose()
        {
            ProductService.ProductsChanged -= StateHasChanged;
        }
    }
}
