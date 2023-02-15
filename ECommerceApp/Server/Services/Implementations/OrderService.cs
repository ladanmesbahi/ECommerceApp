using System.Security.Claims;

namespace ECommerceApp.Server.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;
        private readonly ICartService _cartService;
        private readonly IHttpContextAccessor _httpContext;

        public OrderService(DataContext context, ICartService cartService, IHttpContextAccessor httpContext)
        {
            _context = context;
            _cartService = cartService;
            _httpContext = httpContext;
        }

        private int GetUserId() => int.Parse(_httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public async Task<ServiceResponse<bool>> PlaceOrder()
        {
            var products = (await _cartService.GetDbCartProducts()).Data;
            decimal totalPrice = 0;
            products.ForEach(product => totalPrice += product.Price * product.Quantity);

            var orderItems = new List<OrderItem>();
            products.ForEach(product => orderItems.Add(new OrderItem
            {
                ProductId = product.ProductId,
                ProductTypeId = product.ProductTypeId,
                Quantity = product.Quantity,
                TotalPrice = product.Price * product.Quantity
            }));

            var order = new Order
            {
                UserId = GetUserId(),
                OrderDate = DateTime.Now,
                OrderItems = orderItems,
                TotalPrice = totalPrice
            };

            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(_context.CartItems.Where(cartItem => cartItem.UserId == GetUserId()));

            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }
    }
}
