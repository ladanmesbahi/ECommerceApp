using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Server.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly DataContext _context;

        public CartService(DataContext context)
        {
            _context = context;
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
    }
}
