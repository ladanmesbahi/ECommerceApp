using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Server.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _context;

        public CategoryService(DataContext context)
        {
            _context = context;
        }
        public async Task<ServiceResponse<List<Category>>> GetCategories()
        {
            return new ServiceResponse<List<Category>>
            {
                Data = await _context.Set<Category>().ToListAsync()
            };
        }
    }
}
