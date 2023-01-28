using ECommerceApp.Shared.ViewModels;

namespace ECommerceApp.Client.Shared
{
    public partial class CartCounter : IDisposable
    {
        private int GetCartItemsCount()
        {
            return LocalStorage.GetItem<List<CartItem>>("cart")?.Count ?? 0;
        }

        protected override void OnInitialized()
        {
            CartService.OnChange += StateHasChanged;
        }
        public void Dispose()
        {
            CartService.OnChange -= StateHasChanged;
        }
    }
}
