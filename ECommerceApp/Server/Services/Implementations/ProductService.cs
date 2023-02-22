using ECommerceApp.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Server.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ServiceResponse<List<Product>>> GetProducts()
        {
            return new ServiceResponse<List<Product>>
            {
                Data = await _context.Products
                    .Where(p => p.Visible && !p.Deleted)
                    .Include(p => p.Variants.Where(v => v.Visible && !v.Deleted))
                    .ToListAsync()
            };
        }

        public async Task<ServiceResponse<Product>> GetProductById(int id)
        {
            var response = new ServiceResponse<Product>();
            Product product;

            if (_httpContextAccessor.HttpContext.User.IsInRole("Admin"))

                product = await _context.Products
               .Include(p => p.Variants.Where(v => !v.Deleted))
               .ThenInclude(v => v.ProductType)
               .FirstOrDefaultAsync(p => p.Id == id && !p.Deleted);
            else
                product = await _context.Products
                   .Include(p => p.Variants.Where(v => v.Visible && !v.Deleted))
                   .ThenInclude(v => v.ProductType)
                   .FirstOrDefaultAsync(p => p.Id == id && !p.Deleted && p.Visible);
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
                    .Where(p => p.Category.Url.ToLower().Equals(categoryUrl.ToLower()) && p.Visible && !p.Deleted)
                    .Include(p => p.Variants.Where(v => v.Visible && !v.Deleted))
                    .ToListAsync()
            };
        }

        public async Task<ServiceResponse<ProductSearchResult>> SearchProducts(string searchText, int page)
        {
            var pageSize = 2f;
            var pageCount = Math.Ceiling((await FindProductsBySearchText(searchText)).Count / pageSize);
            var products = await _context.Products
                .Where(p => (p.Title.ToLower().Contains(searchText.ToLower()) ||
                            p.Description.ToLower().Contains(searchText.ToLower())
                            && p.Visible && !p.Deleted))
                .Include(p => p.Variants.Where(v => v.Visible && !v.Deleted))
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
                .Where(p => (p.Title.ToLower().Contains(searchText.ToLower()) ||
                            p.Description.ToLower().Contains(searchText.ToLower())
                            && p.Visible && !p.Deleted))
                .Include(p => p.Variants.Where(v => v.Visible && !v.Deleted))
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
                    .Where(p => p.Featured && p.Visible && !p.Deleted)
                    .Include(p => p.Variants.Where(v => v.Visible && !v.Deleted))
                    .ToListAsync()
            };
        }

        public async Task<ServiceResponse<List<Product>>> GetAdminProducts()
        {
            return new ServiceResponse<List<Product>>
            {
                Data = await _context.Products
                    .Where(p => !p.Deleted)
                    .Include(p => p.Variants.Where(v => !v.Deleted))
                    .ThenInclude(v => v.ProductType)
                    .ToListAsync()
            };
        }

        public async Task<ServiceResponse<Product>> CreateProduct(Product product)
        {
            foreach (var variant in product.Variants)
                variant.ProductType = null;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return new ServiceResponse<Product>
            {
                Data = product
            };
        }

        public async Task<ServiceResponse<Product>> UpdateProduct(Product product)
        {
            var dbProduct = await _context.Products.FindAsync(product.Id);
            if (dbProduct == null)
                return new ServiceResponse<Product>
                {
                    Success = false,
                    Message = "Product not found."
                };
            dbProduct.Title = product.Title;
            dbProduct.Description = product.Description;
            dbProduct.ImageUrl = product.ImageUrl;
            dbProduct.CategoryId = product.CategoryId;
            dbProduct.Visible = product.Visible;
            dbProduct.Featured = product.Featured;

            foreach (var variant in product.Variants)
            {
                var dbVariant = await _context.ProductVariants
                    .SingleOrDefaultAsync(v => v.ProductId == variant.ProductId && v.ProductTypeId == variant.ProductTypeId);
                if (dbVariant is null)
                {
                    variant.ProductType = null;
                    _context.ProductVariants.Add(variant);
                }
                else
                {
                    dbVariant.ProductTypeId = variant.ProductTypeId;
                    dbVariant.Price = variant.Price;
                    dbVariant.OriginalPrice = variant.OriginalPrice;
                    dbVariant.Visible = variant.Visible;
                    dbVariant.Deleted = variant.Deleted;
                }
            }

            await _context.SaveChangesAsync();
            return new ServiceResponse<Product>
            {
                Data = product
            };
        }

        public async Task<ServiceResponse<bool>> DeleteProduct(int productId)
        {
            var dbProduct = await _context.Products.FindAsync(productId);
            if (dbProduct == null)
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Data = false,
                    Message = "Product not found."
                };
            dbProduct.Deleted = true;
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool>
            {
                Success = true,
                Data = true
            };
        }
    }
}
