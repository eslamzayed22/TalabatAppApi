using Microsoft.AspNetCore.Mvc;
using ServiceAbstractionLayer;
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            var products = await _serviceManager.ProductService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var product = await _serviceManager.ProductService.GetProductByIdAsync(id);
            return Ok(product);
        }

        //Get Brands
        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllBrands() 
        { 
            var brands = await _serviceManager.ProductService.GetAllBrandsAsync();
            return Ok(brands);
        }

        //Get Types
        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllTypes()
        {
            var types = await _serviceManager.ProductService.GetAllTypesAsync();
            return Ok(types);
        }


    }
}
