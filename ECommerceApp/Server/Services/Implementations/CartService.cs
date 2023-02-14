using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ECommerceApp.Server.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<bool>> AddToCart(CartItem cartItem)
        {
            cartItem.UserId = GetUserId();
            var sameItem = await _context.CartItems.FirstOrDefaultAsync(ci => ci.ProductId == cartItem.ProductId && ci.ProductTypeId == cartItem.ProductTypeId && ci.UserId == cartItem.UserId);
            if (sameItem is null)
                _context.CartItems.Add(cartItem);
            else
                sameItem.Quantity += cartItem.Quantity;
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<int>> GetCartItemsCount()
        {
            var count = (await _context.CartItems.Where(ci => ci.UserId == GetUserId()).ToListAsync()).Count;
            return new ServiceResponse<int> { Data = count };
        }
        public async Task<ServiceResponse<List<CartProductResponse>>> GetCartProducts(List<CartItem> cartItems)
        {
            var result = new ServiceResponse<List<CartProductResponse>>
            {
                Data = new List<CartProductResponse>()
            };

            foreach (var item in cartItems)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
                if (product is null)
                    continue;
                var variant = await _context.ProductVariants.Where(v =>
                    v.ProductId == product.Id && v.ProductTypeId == item.ProductTypeId)
                    .Include(v => v.ProductType)
                    .FirstOrDefaultAsync();
                if (variant is null)
                    continue;
                result.Data.Add(new CartProductResponse
                {
                    ProductId = product.Id,
                    Title = product.Title,
                    ImageUrl = product.ImageUrl,
                    Price = variant.Price,
                    ProductTypeId = variant.ProductTypeId,
                    ProductType = variant.ProductType.Name,
                    Quantity = item.Quantity
                });
            }
            return result;
        }

        public async Task<ServiceResponse<List<CartProductResponse>>> GetDbCartProducts()
        {
            return await GetCartProducts(await _context.CartItems.Where(ci => ci.UserId == GetUserId()).ToListAsync());
        }

        public async Task<ServiceResponse<bool>> RemoveFromCart(int productId, int productTypeId)
        {
            var dbCartItem = await _context.CartItems
               .FirstOrDefaultAsync(ci => ci.ProductId == productId && ci.ProductTypeId == productTypeId && ci.UserId == GetUserId());
            if (dbCartItem is null)
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Data = false,
                    Message = "Cart item does not exist."
                };
            _context.CartItems.Remove(dbCartItem);
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<List<CartProductResponse>>> StoreCartItems(List<CartItem> cartItems)
        {
            cartItems.ForEach(cartItem => cartItem.UserId = GetUserId());
            _context.CartItems.AddRange(cartItems);
            await _context.SaveChangesAsync();
            return await GetDbCartProducts();
        }

        public async Task<ServiceResponse<bool>> UpdateQuantity(CartItem cartItem)
        {
            var dbCartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == cartItem.ProductId && ci.ProductTypeId == cartItem.ProductTypeId && ci.UserId == GetUserId());
            if (dbCartItem is null)
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Data = false,
                    Message = "Cart item does not exist."
                };
            dbCartItem.Quantity = cartItem.Quantity;
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool> { Data = true };
        }

        private int GetUserId()
        {
            return int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
