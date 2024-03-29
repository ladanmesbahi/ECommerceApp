﻿using ECommerceApp.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProducts()
        {
            return Ok(await _productService.GetProducts());
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<ServiceResponse<Product>>> GetProductById(int productId)
        {
            return Ok(await _productService.GetProductById(productId));
        }

        [HttpGet("category/{categoryUrl}")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductsByCategory(string categoryUrl)
        {
            return Ok(await _productService.GetProductsByCategory(categoryUrl));
        }

        [HttpGet("search/{searchText}/{page}")]
        public async Task<ActionResult<ServiceResponse<ProductSearchResult>>> Search(string searchText, int page = 1)
        {
            return Ok(await _productService.SearchProducts(searchText, page));
        }

        [HttpGet("searchSuggestions/{searchText}")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductSearchSuggestions(string searchText)
        {
            return Ok(await _productService.GetProductSearchSuggestions(searchText));
        }

        [HttpGet("featured")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetFeaturedProducts()
        {
            return Ok(await _productService.GetFeaturedProducts());
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetAdminProducts()
        {
            return Ok(await _productService.GetAdminProducts());
        }

        [HttpPost("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<Product>>> CreateProduct(Product product)
        {
            return Ok(await _productService.CreateProduct(product));
        }

        [HttpPut("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<Product>>> UpdateProduct(Product product)
        {
            return Ok(await _productService.UpdateProduct(product));
        }

        [HttpDelete("admin/{productId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteProduct(int productId)
        {
            return Ok(await _productService.DeleteProduct(productId));
        }
    }
}
