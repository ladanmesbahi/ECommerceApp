namespace ECommerceApp.Client.Shared
{
    public partial class UserButton
    {
        private bool _showUserMenu = false;
        private string? UserMenuCssClass => _showUserMenu ? "show-menu" : null;

        private void ToggleUserMenu()
        {
            _showUserMenu = !_showUserMenu;
        }

        private async Task HideUserMenu()
        {
            await Task.Delay(200);
            _showUserMenu = false;
        }
        private async Task Logout()
        {
            await LocalStorageService.RemoveItemAsync("authToken");
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            NavigationManager.NavigateTo("");
        }
    }
}
