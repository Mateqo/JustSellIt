using AutoMapper;
using JustSellIt.Application.Interfaces;
using JustSellIt.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace JustSellIt.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAplication(this IServiceCollection services)
        {
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<IStatusService, StatusService>();
            services.AddTransient<IAdminService, AdminService>();
            services.AddTransient<IOwnerService, OwnerService>();
            services.AddTransient<IOwnerContactService, OwnerContactService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
