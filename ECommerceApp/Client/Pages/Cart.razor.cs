using ECommerceApp.Shared.ViewModels;
using Microsoft.AspNetCore.Components;

namespace ECommerceApp.Client.Pages
{
    public partial class Cart
    {
        private List<CartProductResponse> cartProducts = null;
        private string message = "Loading cart...";
        protected override async Task OnInitializedAsync()
        {
            await LoadCart();
        }

        private async Task LoadCart()
        {
            await CartService.GetCartItemsCount();
            cartProducts = await CartService.GetCartProducts();
            if (cartProducts.Count == 0)
                message = "Your cart is empty.";
        }

        private async Task RemoveProductFromCart(int productId, int productTypeId)
        {
            await CartService.RemoveProductFromCart(productId, productTypeId);
            await LoadCart();
        }

        private async Task UpdateQuantity(ChangeEventArgs e, CartProductResponse product)
        {
            product.Quantity = int.Parse(e.Value.ToString());
            if (product.Quantity < 1)
                product.Quantity = 1;
            await CartService.UpdateQuantity(product);
        }
    }
}
