using Microsoft.Extensions.DependencyInjection;
using ServiceAbstractionLayer;
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
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddAutoMapper((x) => { }, typeof(ServiceAssemblyReference).Assembly);
            return services;
        }
    }
}
