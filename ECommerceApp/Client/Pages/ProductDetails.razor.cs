using Microsoft.AspNetCore.Components;

namespace ECommerceApp.Client.Pages
{
    public partial class ProductDetails
    {
        private Product? _product = null;
        private string _message;
        private int _currentTypeId = 1;

        [Parameter]
        public int Id { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            _message = "Loading product...";
            var response = await ProductService.GetProductById(Id);
            if (!response.Success)
                _message = response.Message;
            else
            {
                _product = response.Data;
                if (_product.Variants.Count > 0)
                    _currentTypeId = _product.Variants.First().ProductTypeId;
            }
        }

        private ProductVariant GetSelectedVariant()
        {
            return _product.Variants.SingleOrDefault(v => v.ProductTypeId == _currentTypeId);
        }
    }
}
