using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ProductQueryParameter
    {
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public ProductSortingOptions Sort { get; set; }
        public string? Search { get; set; }


        #region Pagination
        private const int DefaultPageSize = 5;
        private const int MaxPageSize = 10;

        public int PageNumber { get; set; } = 1;

        private int pageSize = DefaultPageSize;
        public int PageSize
        {      
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }
        #endregion

    }
}
