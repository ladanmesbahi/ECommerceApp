namespace ECommerceApp.Client.Pages.Admin
{
    public partial class Products
    {
        protected override async Task OnInitializedAsync()
        {
            await ProductService.GetAdminProducts();
        }
        void EditProduct(int productId)
        {
            NavigationManager.NavigateTo($"admin/product/{productId}");
        }
        void CreateProduct()
        {
            NavigationManager.NavigateTo($"admin/product");
        }
    }
}
