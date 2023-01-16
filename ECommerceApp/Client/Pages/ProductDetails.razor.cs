using Microsoft.AspNetCore.Components;

namespace ECommerceApp.Client.Pages
{
    public partial class ProductDetails
    {
        private Product? _product = null;
        private string message;

        [Parameter]
        public int Id { get; set; }


        protected override async Task OnParametersSetAsync()
        {
            message = "Loading product...";
            var response = await ProductService.GetProductById(Id);
            if (!response.Success)
                message = response.Message;
            else
                _product = response.Data;
        }
    }
}
