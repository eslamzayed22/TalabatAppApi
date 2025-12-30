using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models.OrderModels;
using Microsoft.Extensions.Configuration;
using ServiceAbstractionLayer;
using Shared.DataTransferObjects.BasketDtos;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = DomainLayer.Models.ProductModels.Product;

namespace ServiceLayer.Services
{
    public class PaymentService(IConfiguration _configuration, IBasketRepository _basketRepository,
                            IUnitOfWork _unitOfWork, IMapper _mapper) : IPaymentService
    {
        public async Task<BasketDto> CreateOrUpdatePaymentIntent(string basketId)
        {
            //Configure Stripe --> Install -Package Stripe.net 
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            //Get Basket By BasketId
            var basket = await _basketRepository.GetBasketAsync(basketId) ?? throw new BasketNotFoundException(basketId);

            //Get Amount - Get Products Amount + Delivery Cost
            var productRepo = _unitOfWork.GetRepository<Product, int>();
            foreach (var item in basket.Items)
            {
                var product = await productRepo.GetByIdAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);
                item.Price = product.Price;
                
            }
            ArgumentNullException.ThrowIfNull(basket.DeliveryMethodId);
            var deliveryMethods = await _unitOfWork.GetRepository<DeliveryMethod, int>()
                                    .GetByIdAsync(basket.DeliveryMethodId.Value) 
                                    ?? throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);

            basket.ShippingPrice = deliveryMethods.Price;

            var basketAmount = (long) (basket.Items.Sum(item => item.Price * item.Quantity) + deliveryMethods.Price) * 100;  

            //Create Payment Intent
            var _stripePaymentIntentService = new PaymentIntentService();

            if(basket.PaymentIntentId is null)
            { //Create
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = basketAmount,
                    Currency = "USD",
                    PaymentMethodTypes = ["card"]             
                };
                var paymentIntent = await _stripePaymentIntentService.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            { //Update
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = basketAmount
                };
                await _stripePaymentIntentService.UpdateAsync(basket.PaymentIntentId, options);
            }
            await _basketRepository.CreateOrUpdateBasketAsync(basket);
            return _mapper.Map<BasketDto>(basket);

        }
    }
}
