using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Server.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;

        public OrderService(DataContext context, ICartService cartService, IAuthService authService)
        {
            _context = context;
            _cartService = cartService;
            _authService = authService;
        }

        public async Task<ServiceResponse<OrderDetailsResponse>> GetOrderDetails(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.ProductType)
                .SingleOrDefaultAsync(o => o.Id == orderId && o.UserId == _authService.GetUserId());

            if (order == null)
                return new ServiceResponse<OrderDetailsResponse>
                {
                    Success = false,
                    Message = "order not found"
                };

            return new ServiceResponse<OrderDetailsResponse>
            {
                Data = new OrderDetailsResponse
                {
                    OrderDate = order.OrderDate,
                    TotalPrice = order.OrderItems.Sum(item => item.TotalPrice),
                    Products = order.OrderItems.Select(item => new OrderDetailsProductResponse
                    {
                        ProductId = item.ProductId,
                        ProductType = item.ProductType.Name,
                        TotalPrice = item.TotalPrice,
                        Quantity = item.Quantity,
                        Title = item.Product.Title,
                        ImageUrl = item.Product.ImageUrl
                    }).ToList()
                }
            };
        }

        public async Task<ServiceResponse<List<OrderOverviewResponse>>> GetOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == _authService.GetUserId())
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
            return new ServiceResponse<List<OrderOverviewResponse>>
            {
                Data = orders.Select(o => new OrderOverviewResponse
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    TotalPrice = o.TotalPrice,
                    Product = o.OrderItems.Count > 1 ? $"{o.OrderItems.First().Product.Title} and {o.OrderItems.Count - 1} more..." :
                    o.OrderItems.First().Product.Title,
                    ProductImageUrl = o.OrderItems.First().Product.ImageUrl
                }).ToList()
            };
        }

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
                UserId = _authService.GetUserId(),
                OrderDate = DateTime.Now,
                OrderItems = orderItems,
                TotalPrice = totalPrice
            };

            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(_context.CartItems.Where(cartItem => cartItem.UserId == _authService.GetUserId()));

            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }
    }
}
