namespace ECommerceApp.Server.Services.Abstractions
{
    public interface ICartService
    {
        Task<ServiceResponse<List<CartProductResponse>>> GetCartProducts(List<CartItem> cartItems);
    }
}
