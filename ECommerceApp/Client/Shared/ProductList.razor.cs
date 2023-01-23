namespace ECommerceApp.Client.Shared
{
    public partial class ProductList : IDisposable
    {
        protected override void OnInitialized()
        {
            ProductService.ProductsChanged += StateHasChanged;
        }

        public void Dispose()
        {
            ProductService.ProductsChanged -= StateHasChanged;
        }

        private string GetPriceText(Product product)
        {
            return product.Variants.Count == 0 ? string.Empty :
                product.Variants.Count == 1 ? $"${product.Variants.Single().Price}" :
                $"Starting at ${product.Variants.Min(p => p.Price)}";
        }
    }
}
