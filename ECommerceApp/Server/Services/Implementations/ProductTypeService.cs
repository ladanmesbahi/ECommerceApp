﻿using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Server.Services.Implementations
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly DataContext _context;

        public ProductTypeService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<ProductType>>> AddProductType(ProductType productType)
        {
            productType.IsNew = productType.Editing = false;
            _context.ProductTypes.Add(productType);
            await _context.SaveChangesAsync();
            return await GetProductTypes();
        }

        public async Task<ServiceResponse<List<ProductType>>> GetProductTypes()
        {
            return new ServiceResponse<List<ProductType>>
            {
                Data = await _context.ProductTypes.ToListAsync()
            };
        }

        public async Task<ServiceResponse<List<ProductType>>> UpdateProductType(ProductType productType)
        {
            var dbProductType = await _context.ProductTypes.FindAsync(productType.Id);
            if (dbProductType == null)
                return new ServiceResponse<List<ProductType>> { Success = false, Message = "Product type not found." };
            dbProductType.Name = productType.Name;
            await _context.SaveChangesAsync();
            return await GetProductTypes();
        }
    }
}
