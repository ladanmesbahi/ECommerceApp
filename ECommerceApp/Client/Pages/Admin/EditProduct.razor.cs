using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ECommerceApp.Client.Pages.Admin
{
    public partial class EditProduct
    {
        [Parameter]
        public int Id { get; set; }

        Product product = new Product();
        bool loading = true;
        string btnText = string.Empty;
        string message = "Loading ...";

        protected override async Task OnInitializedAsync()
        {
            await ProductTypeService.GetProductTypes();
            await CategoryService.GetAdminCategories();
        }

        protected override async Task OnParametersSetAsync()
        {
            if (Id == 0)
            {
                product = new Product { IsNew = true };
                btnText = "Create Product";
            }
            else
            {
                var dbProduct = (await ProductService.GetProductById(Id)).Data;

                if (dbProduct is null)
                {
                    message = "Product does not exist";
                    return;
                }
                product = dbProduct;
                product.Editing = true;
                btnText = "Update Product";
            }
            loading = false;
        }
        void RemoveVariant(int productTypeId)
        {
            var variant = product.Variants.Find(v => v.ProductTypeId == productTypeId);
            if (variant == null)
                return;
            if (variant.IsNew)
                product.Variants.Remove(variant);
            else
                variant.Deleted = true;
        }
        void AddVariant()
        {
            product.Variants.Add(new ProductVariant { IsNew = true, ProductId = product.Id });
        }

        async Task AddOrUpdateProduct()
        {
            if (product.IsNew)
            {
                var result = await ProductService.CreateProduct(product);
                NavigationManager.NavigateTo($"/admin/product/{result.Id}");
            }
            else
            {
                product.IsNew = false;
                product = await ProductService.UpdateProduct(product);
                NavigationManager.NavigateTo($"/admin/product/{product.Id}", true);
            }
        }

        async Task DeleteProduct()
        {
            bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", $"Are you sure to delete '{product.Title}'?");
            if (confirmed)
            {
                await ProductService.DeleteProduct(product.Id);
                NavigationManager.NavigateTo("/admin/products");
            }
        }
    }
}
