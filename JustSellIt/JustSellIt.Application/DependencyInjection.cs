using AutoMapper;
using JustSellIt.Application.Interfaces;
using JustSellIt.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace JustSellIt.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAplication(this IServiceCollection services)
        {
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IStatusService, StatusService>();
            services.AddTransient<IAdminService, AdminService>();
            services.AddTransient<IFavoriteService, FavoriteService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
