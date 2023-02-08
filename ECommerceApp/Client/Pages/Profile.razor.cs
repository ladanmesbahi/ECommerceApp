using ECommerceApp.Shared.Dtos;

namespace ECommerceApp.Client.Pages
{
    public partial class Profile
    {
        private UserChangePassword _request = new UserChangePassword();
        private string _message = string.Empty;

        private async Task ChangePassword()
        {
            var result = await AuthService.ChangePassword(_request);
            _message = result.Message;
        }
    }
}
