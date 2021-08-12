using MasterDev.Domain.Interfaces;
using MasterDev.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasterDev.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IKlientRepository, KlientRepository>();
            


            return services;
        }
    }
}
