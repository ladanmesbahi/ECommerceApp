namespace ECommerceApp.Client.Shared
{
    public partial class CartCounter : IDisposable
    {
        private int GetCartItemsCount()
        {
            return LocalStorage.GetItem<int>("cartItemsCount");
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
