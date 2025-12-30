using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models.OrderModels;
using DomainLayer.Models.ProductModels;
using ServiceAbstractionLayer;
using ServiceLayer.Specifications.OrderModelsSpecifications;
using Shared.DataTransferObjects.IdentityDtos;
using Shared.DataTransferObjects.OrderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class OrderService(IMapper _mapper,
                        IBasketRepository _basketRepository,
                        IUnitOfWork _unitOfWork) : IOrderService
    {
        public async Task<OrderToReturnDto> CreateOrderAsync(OrderDto orderDto, string email)
        {
            // Map Address to OrderAddress
            var orderAddress = _mapper.Map<AddressDto, OrderAddress>(orderDto.ShipToAddress);

            var basket = await _basketRepository.GetBasketAsync(orderDto.BasketId)
                        ?? throw new BasketNotFoundException(orderDto.BasketId);

            ArgumentNullException.ThrowIfNull(basket.PaymentIntentId);
            var _orderRepo = _unitOfWork.GetRepository<Order, Guid>();
            var existingOrder = await _orderRepo.GetByIdAsync(new OrderWithPaymentIntentSpecifications(basket.PaymentIntentId));
            if (existingOrder is not null)
            {
                _orderRepo.Remove(existingOrder);
            }

            //Create order items List From Basket Items
            List<OrderItem> OrderItems = [];
            var productRepo = _unitOfWork.GetRepository<Product, int>();

            foreach (var baketitem in basket.Items)
            {
                var product = await productRepo.GetByIdAsync(baketitem.Id)
                            ?? throw new ProductNotFoundException(baketitem.Id);

                var orderItem = new OrderItem()
                {
                    Product = new ProductItemOrdered()
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        PictureUrl = product.PictureUrl,
                    },
                    Price = product.Price,
                    Quantity = baketitem.Quantity
                };
                OrderItems.Add(orderItem);
            }

            //Get Delivery Method
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>()
                                                  .GetByIdAsync(orderDto.DeliveryMethodId)
                                     ?? throw new DeliveryMethodNotFoundException(orderDto.DeliveryMethodId);

            //Calculate Subtotal
            var subTotal = OrderItems.Sum(i => i.Quantity * i.Price);

            var order = new Order(email, orderAddress, deliveryMethod, deliveryMethod.Id, subTotal, OrderItems, basket.PaymentIntentId);
            
            await _orderRepo.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<OrderToReturnDto>(order);
        }

        public async Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await  _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<DeliveryMethodDto>>(deliveryMethods);
        }

        public async Task<IEnumerable<OrderToReturnDto>> GetAllOrdersAsync(string email)
        {
            var specs = new OrderSpecifications(email);
            var _orderRepo = _unitOfWork.GetRepository<Order, Guid>();
            var orders = _orderRepo.GetAllAsync(specs);
            return _mapper.Map<IEnumerable<OrderToReturnDto>>(orders);

        }

        public async Task<OrderToReturnDto> GetOrderAsync(Guid id)
        {
            var specs = new OrderSpecifications(id);
            var _orderRepo = _unitOfWork.GetRepository<Order, Guid>();
            var order = _orderRepo.GetByIdAsync(specs);
            return _mapper.Map<OrderToReturnDto>(order);

        }
    }
}
