using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Server.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }
        public async Task<ServiceResponse<List<Product>>> GetProducts()
        {
            return new ServiceResponse<List<Product>>
            {
                Data = await _context.Products.ToListAsync()
            };
        }
    }
}
