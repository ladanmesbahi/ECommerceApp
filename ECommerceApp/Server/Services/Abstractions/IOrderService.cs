namespace ECommerceApp.Server.Services.Abstractions
{
    public interface IOrderService
    {
        Task<ServiceResponse<bool>> PlaceOrder();
    }
}
