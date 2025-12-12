using Microsoft.AspNetCore.Mvc;
using ServiceAbstractionLayer;
using Shared;
using Shared.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController (IServiceManager _serviceManager) : ControllerBase
    { 
        [HttpGet] //Get BaseUrl/api/products
        public async Task<ActionResult<PaginatedResult<ProductDto>>> GetAllProducts([FromQuery]ProductQueryParameter queryParameter)
        {
            var products = await _serviceManager.ProductService.GetAllProductsAsync(queryParameter);
            return Ok(products);
        }

        [HttpGet("{id:int}")] //Get BaseUrl/api/products/3
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var product = await _serviceManager.ProductService.GetProductByIdAsync(id);
            return Ok(product);
        }

        //Get Brands
        [HttpGet("brands")] //Get BaseUrl/api/products/brands
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllBrands() 
        { 
            var brands = await _serviceManager.ProductService.GetAllBrandsAsync();
            return Ok(brands);
        }

        //Get Types
        [HttpGet("types")]  //Get BaseUrl/api/products/types
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllTypes()
        {
            var types = await _serviceManager.ProductService.GetAllTypesAsync();
            return Ok(types);
        }


    }
}
