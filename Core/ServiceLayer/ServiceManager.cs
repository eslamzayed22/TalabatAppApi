using AutoMapper;
using DomainLayer.Contracts;
using ServiceAbstractionLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class ServiceManager (IUnitOfWork _unitOfWork, IMapper _mapper): IServiceManager
    {
        private readonly Lazy<IProductService> _lazyProductService =
            new Lazy<IProductService>(() => new ProductService(_unitOfWork, _mapper));
        public IProductService ProductService => _lazyProductService.Value;
    }
}
