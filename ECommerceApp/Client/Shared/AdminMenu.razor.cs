using System.Security.Claims;

namespace ECommerceApp.Client.Shared
{
    public partial class AdminMenu
    {
        bool isAuthorized = false;
        protected override async Task OnInitializedAsync()
        {
            string role = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            isAuthorized = (role.Contains("Admin"));
        }
    }
}
