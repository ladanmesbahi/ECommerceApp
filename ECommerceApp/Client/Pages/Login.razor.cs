using ECommerceApp.Shared.ViewModels;

namespace ECommerceApp.Client.Pages
{
    public partial class Login
    {
        private UserLogin _user = new UserLogin();
        private string _errorMessage = string.Empty;
        private async Task HandleLogin()
        {
            var result = await AuthService.Login(_user);
            if (result.Success)
            {
                _errorMessage = string.Empty;
                await LocalStorage.SetItemAsync("authToken", result.Data);
                NavigationManager.NavigateTo("");
            }
            else
                _errorMessage = result.Message;
        }
    }
}
