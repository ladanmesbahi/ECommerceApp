namespace ECommerceApp.Client.Pages.Admin
{
    public partial class ProductTypes
    {
        ProductType editingProductType = null;
        protected override async Task OnInitializedAsync()
        {
            await ProductTypeService.GetProductTypes();
            ProductTypeService.OnChange += StateHasChanged;
        }
        private void EditProductType(ProductType productType)
        {
            productType.Editing = true;
            editingProductType = productType;
        }
        private void CreateNewProductType()
        {
            editingProductType = ProductTypeService.CreateNewProductType();
        }
        private async Task UpdateProductType()
        {
            if (editingProductType.IsNew)
                await ProductTypeService.AddProductType(editingProductType);
            else
                await ProductTypeService.UpdateProductType(editingProductType);
            editingProductType = new ProductType();
        }
        public void Dispose()
        {
            ProductTypeService.OnChange -= StateHasChanged;
        }
    }
}
