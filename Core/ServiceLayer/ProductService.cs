using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Models.ProductModels;
using ServiceAbstractionLayer;
using Shared.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper) : IProductService
    {
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var repo = _unitOfWork.GetRepository<Product, int>();
            var res = await repo.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(res);
        }
        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Product, int>();
            var product = await repo.GetByIdAsync(id);
            return _mapper.Map<ProductDto>(product);
        }
        public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
        {
            var repo = _unitOfWork.GetRepository<ProductBrand, int>();
            var res = await repo.GetAllAsync();
            return _mapper.Map<IEnumerable<BrandDto>>(res);
        }


        public async Task<IEnumerable<TypeDto>> GetAllTypesAsync()
        {
            var repo = _unitOfWork.GetRepository<ProductType, int>();
            var res = await repo.GetAllAsync();
            return _mapper.Map<IEnumerable<TypeDto>>(res);
        }

    }
}
