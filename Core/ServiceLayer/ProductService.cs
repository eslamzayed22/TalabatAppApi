using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models.ProductModels;
using ServiceAbstractionLayer;
using ServiceLayer.Specifications;
using Shared;
using Shared.DataTransferObjects.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper) : IProductService
    {
        public async Task<PaginatedResult<ProductDto>> GetAllProductsAsync(ProductQueryParameter queryParameter)
        {
            var repo = _unitOfWork.GetRepository<Product, int>();
            var specs = new ProductWithBrandAndTypeSpecificaions(queryParameter);
            var res = await repo.GetAllAsync(specs);
            var responseData = _mapper.Map<IEnumerable<ProductDto>>(res);

            var totalCount = await repo.CountAsync(new ProductCountSpecification(queryParameter));
            return new PaginatedResult<ProductDto>(queryParameter.PageIndex, responseData.Count(), totalCount, responseData);
        }
        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Product, int>();
            var specs = new ProductWithBrandAndTypeSpecificaions(id);
            var product = await repo.GetByIdAsync(specs);

            if(product is not null)
               return _mapper.Map<ProductDto>(product);

            throw new ProductNotFoundException(id);
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
