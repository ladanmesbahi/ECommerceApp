using ECommerceApp.Shared.ViewModels;
using Microsoft.AspNetCore.WebUtilities;

namespace ECommerceApp.Client.Pages
{
    public partial class Login
    {
        private UserLogin _user = new UserLogin();
        private string _errorMessage = string.Empty;
        private string _returnUrl = string.Empty;
        protected override void OnInitialized()
        {
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("returnUrl", out var url))
                _returnUrl = url;
        }
        private async Task HandleLogin()
        {
            var result = await AuthService.Login(_user);
            if (result.Success)
            {
                _errorMessage = string.Empty;
                await LocalStorage.SetItemAsync("authToken", result.Data);
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await CartService.StoreCartItems(true);
                await CartService.GetCartItemsCount();
                NavigationManager.NavigateTo(_returnUrl);
            }
            else
                _errorMessage = result.Message;
        }
    }
}
