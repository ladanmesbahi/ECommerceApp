using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<bool>>> PlaceOrder()
        {
            return Ok(await _orderService.PlaceOrder());
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<OrderOverviewResponse>>> GetOrders()
        {
            return Ok(await _orderService.GetOrders());
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<ServiceResponse<OrderOverviewResponse>>> GetOrderDetails(int orderId)
        {
            return Ok(await _orderService.GetOrderDetails(orderId));
        }
    }
}
