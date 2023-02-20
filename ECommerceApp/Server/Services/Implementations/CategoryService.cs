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

        public async Task<ServiceResponse<List<Category>>> AddCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return await GetAdminCategories();
        }

        public async Task<ServiceResponse<List<Category>>> DeleteCategory(int categoryId)
        {
            Category? category = await GetCategoryById(categoryId);
            if (category == null)
                return new ServiceResponse<List<Category>>
                {
                    Success = false,
                    Message = "Category not found"
                };

            category.Deleted = true;
            await _context.SaveChangesAsync();
            return await GetAdminCategories();
        }

        private async Task<Category> GetCategoryById(int categoryId)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
        }

        public async Task<ServiceResponse<List<Category>>> GetAdminCategories()
        {
            return new ServiceResponse<List<Category>>
            {
                Data = await _context.Set<Category>()
                .Where(c => !c.Deleted)
                .ToListAsync()
            };
        }

        public async Task<ServiceResponse<List<Category>>> GetCategories()
        {
            return new ServiceResponse<List<Category>>
            {
                Data = await _context.Set<Category>()
                .Where(c => !c.Deleted && c.Visible)
                .ToListAsync()
            };
        }

        public async Task<ServiceResponse<List<Category>>> UpdateCategory(Category category)
        {
            var dbCategory = await GetCategoryById(category.Id);
            if (dbCategory is null)
                return new ServiceResponse<List<Category>>
                {
                    Success = false,
                    Message = "Category not found."
                };
            dbCategory.Name = category.Name;
            dbCategory.Url = category.Url;
            dbCategory.Visible = category.Visible;

            await _context.SaveChangesAsync();
            return await GetAdminCategories();
        }
    }
}
