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

        public async Task<ServiceResponse<Product>> GetProductById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            var response = new ServiceResponse<Product>();
            if (product is null)
            {
                response.Success = false;
                response.Message = "Product not found";
            }
            else
                response.Data = product;
            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsByCategory(string categoryUrl)
        {
            return new ServiceResponse<List<Product>>
            {
                Data = await _context.Products.Where(p => p.Category.Url.ToLower().Equals(categoryUrl.ToLower())).ToListAsync()
            };
        }
    }
}
