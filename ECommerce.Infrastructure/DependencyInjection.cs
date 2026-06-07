using CleanAPI.Application.Services.Interfaces;
using CleanAPI.Domain.Interfaces;
using CleanAPI.Infrastructure.Data;
using CleanAPI.Infrastructure.Repositorie;
using CleanAPI.Infrastructure.Service;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repositorie;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPI.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Database
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

            // Repositories
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICartRepository, CartRepository> ();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();

            // Token Service
            services.AddSingleton<ITokenServices>(provider =>
                new TokenService(configuration));

            return services;
        }
    }
}
