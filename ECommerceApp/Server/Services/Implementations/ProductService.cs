using ECommerceApp.Shared.Dtos;
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
                Data = await _context.Products
                    .Include(p => p.Variants)
                    .ToListAsync()
            };
        }

        public async Task<ServiceResponse<Product>> GetProductById(int id)
        {
            var product = await _context.Products
                .Include(p => p.Variants)
                .ThenInclude(v => v.ProductType)
                .FirstOrDefaultAsync(p => p.Id == id);
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
                Data = await _context.Products
                    .Include(p => p.Variants)
                    .Where(p => p.Category.Url.ToLower().Equals(categoryUrl.ToLower())).ToListAsync()
            };
        }

        public async Task<ServiceResponse<ProductSearchResult>> SearchProducts(string searchText, int page)
        {
            var pageSize = 2f;
            var pageCount = Math.Ceiling((await FindProductsBySearchText(searchText)).Count / pageSize);
            var products = await _context.Products
                .Where(p => p.Title.ToLower().Contains(searchText.ToLower()) ||
                            p.Description.ToLower().Contains(searchText.ToLower()))
                .Include(p => p.Variants)
                .Skip((page - 1) * (int)pageSize)
                .Take((int)pageSize)
                .ToListAsync(); ;
            return new ServiceResponse<ProductSearchResult>
            {
                Data = new ProductSearchResult
                {
                    Products = products,
                    CurrenPage = page,
                    Pages = (int)pageCount
                }
            };
        }

        private async Task<List<Product>> FindProductsBySearchText(string searchText)
        {
            return await _context.Products
                .Where(p => p.Title.ToLower().Contains(searchText.ToLower()) ||
                            p.Description.ToLower().Contains(searchText.ToLower()))
                .Include(p => p.Variants)
                .ToListAsync();
        }

        public async Task<ServiceResponse<List<string>>> GetProductSearchSuggestions(string searchText)
        {
            var products = await FindProductsBySearchText(searchText);
            var result = new List<string>();

            foreach (var product in products)
            {
                if (product.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    result.Add(product.Title);
                if (product.Description != null)
                {
                    var punctuation = product.Description.Where(char.IsPunctuation).Distinct().ToArray();
                    var words = product.Description.Split().Select(s => s.Trim(punctuation));
                    foreach (var word in words)
                        if (word.Contains(searchText, StringComparison.OrdinalIgnoreCase) && !result.Contains(word))
                            result.Add(word);
                }
            }

            return new ServiceResponse<List<string>> { Data = result };
        }

        public async Task<ServiceResponse<List<Product>>> GetFeaturedProducts()
        {
            return new ServiceResponse<List<Product>>
            {
                Data = await _context.Products
                    .Where(p => p.Featured)
                    .Include(p => p.Variants)
                    .ToListAsync()
            };
        }
    }
}
