using ECommerceApp.Shared.Dtos;

namespace ECommerceApp.Client.Pages
{
    public partial class Register
    {
        private UserRegister _user = new UserRegister();

        private string _message = string.Empty;
        private string _messageCssClass = string.Empty;
        async Task HandleRegistration()
        {
            var result = await AuthService.Register(_user);
            _message = result.Message;
            _messageCssClass = result.Success ? "text-success" : "text-danger";
        }
    }
}
