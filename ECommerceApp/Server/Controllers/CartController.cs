using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("products")]
        public async Task<ActionResult<ServiceResponse<List<CartProductResponse>>>> GetCartProducts(List<CartItem> cartItems)
        {
            return Ok(await _cartService.GetCartProducts(cartItems));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<CartProductResponse>>>> StoreCartItems(List<CartItem> cartItems)
        {
            return Ok(await _cartService.StoreCartItems(cartItems));
        }

        [HttpGet("count")]
        public async Task<ActionResult<ServiceResponse<int>>> GetCartItemsCount()
        {
            return Ok(await _cartService.GetCartItemsCount());
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<CartProductResponse>>>> GetDbCartProducts()
        {
            return Ok(await _cartService.GetDbCartProducts());
        }

        [HttpPost("add")]
        public async Task<ActionResult<ServiceResponse<bool>>> AddToCart(CartItem cartItem)
        {
            return Ok(await _cartService.AddToCart(cartItem));
        }

        [HttpPut("update-quantity")]
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateQuantity(CartItem cartItem)
        {
            return Ok(await _cartService.UpdateQuantity(cartItem));
        }

        [HttpDelete("{productId}/{productTypeId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> RemoveFromCart(int productId, int productTypeId)
        {
            return Ok(await _cartService.RemoveFromCart(productId, productTypeId));
        }
    }
}
