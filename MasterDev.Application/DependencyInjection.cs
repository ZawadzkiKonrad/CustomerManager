using MasterDev.Application.Interfaces;
using MasterDev.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MasterDev.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IKlientService, KlientService>();
            
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
