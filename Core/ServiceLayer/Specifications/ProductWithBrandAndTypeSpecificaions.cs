using DomainLayer.Models.ProductModels;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Specifications
{
    public class ProductWithBrandAndTypeSpecificaions : BaseSpecifications<Product, int>
    {
        // Get All Products with their Brands and Types
        public ProductWithBrandAndTypeSpecificaions(ProductQueryParameter queryParameter) 
            : base(p => (!queryParameter.BrandId.HasValue || p.BrandId == queryParameter.BrandId) && 
                        (!queryParameter.TypeId.HasValue || p.TypeId == queryParameter.TypeId) &&
                        (string.IsNullOrWhiteSpace(queryParameter.SearchValue) || p.Name.ToLower().Contains(queryParameter.SearchValue.ToLower())))
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);

            switch(queryParameter.sortingOption)
            {
                case ProductSortingOptions.nameAsc:
                    AddOrderBy(p => p.Name);
                    break;
                case ProductSortingOptions.nameDesc:
                    AddOrderByDescending(p => p.Name);
                    break;
                case ProductSortingOptions.priceAsc:
                    AddOrderBy(p => p.Price);
                    break;
                case ProductSortingOptions.priceDesc:
                    AddOrderByDescending(p => p.Price);
                    break;
                default:
                    break;
            }

            ApplyPagination(queryParameter.PageSize, queryParameter.PageIndex);
        }

        // Get Product by Id with its Brand and Type
        public ProductWithBrandAndTypeSpecificaions(int id) : base(p => p.Id == id)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }
    }
}
