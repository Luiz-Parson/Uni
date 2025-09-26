using ConnectorAccess.Service.data;
using ConnectorAccess.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;

namespace ConnectorAccess.Service.config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ConnectorDbContext>(options =>
                options.UseSqlServer("Data Source=localhost;Initial Catalog=ConnectorFasano;User Id=sa;Password=123456"));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<AccessControlService>();
            services.AddScoped<ExclusionControlService>();
            services.AddScoped<ProductService>();
            services.AddScoped<SystemUserService>();

            return services;
        }
    }
}
