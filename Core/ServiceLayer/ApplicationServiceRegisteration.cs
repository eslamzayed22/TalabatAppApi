using Microsoft.Extensions.DependencyInjection;
using ServiceAbstractionLayer;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public static class ApplicationServiceRegisteration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager, ServiceManagerWithFactoryDelegate>();
            services.AddAutoMapper((x) => { }, typeof(ServiceAssemblyReference).Assembly);

            #region Service Registeration

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<Func<IProductService>>(provider => ()
            => provider.GetRequiredService<IProductService>());

            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<Func<IBasketService>>(provider => ()
            => provider.GetRequiredService<IBasketService>());

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<Func<IAuthenticationService>>(provider => ()
            => provider.GetRequiredService<IAuthenticationService>());

            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<Func<IOrderService>>(provider => ()
            => provider.GetRequiredService<IOrderService>()); 

            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<Func<IPaymentService>>(provider => ()
            => provider.GetRequiredService<IPaymentService>()); 

            #endregion

            return services;
        }
    }
}
