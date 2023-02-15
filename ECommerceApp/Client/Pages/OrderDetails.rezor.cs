using ECommerceApp.Shared.ViewModels;
using Microsoft.AspNetCore.Components;

namespace ECommerceApp.Client.Pages
{
    public partial class OrderDetails
    {
        [Parameter]
        public int OrderId { get; set; }

        OrderDetailsResponse order = null;

        protected override async Task OnInitializedAsync()
        {
            order = await OrderService.GetOrderDetails(OrderId);
        }
    }
}
