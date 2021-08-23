using JustSellIt.Domain.Interface;
using JustSellIt.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace JustSellIt.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IImageRepository, ImageRepository>();

            return services;
        }
    }
}
