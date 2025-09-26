using ConnectorAccess.Service.config;
using ConnectorAccess.Service.data;
using ConnectorAccess.Service.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace ConnectorAccess.Service
{
    public class Program
    {
        private static string configFile = "appsettings.json";
        private static string basePath = "";

        public static void Main(string[] args)
        {
            basePath = AppContext.BaseDirectory;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile(configFile, optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            try
            {
                Log.Information("Iniciando a aplicação...");

                var builder = WebApplication.CreateBuilder(args);

                builder.Host.UseSerilog();

                builder.Host.UseWindowsService();

                builder.Services.AddControllers();

                builder.Services.AddDbContext<ConnectorDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

                DependencyInjectionConfig.ConfigureServices();

                builder.Services.AddScoped<ProductService>();
                builder.Services.AddScoped<AccessControlService>();
                builder.Services.AddScoped<GeneralControlService>();
                builder.Services.AddScoped<ExclusionControlService>();
                builder.Services.AddScoped<AccessControlDay>();
                builder.Services.AddScoped<ReportService>();
                builder.Services.AddScoped<SystemUserService>();
                builder.Services.AddScoped<EncryptionHelper>();
                builder.Services.Configure<EmailService>(builder.Configuration.GetSection("EmailSettings"));
                builder.Services.AddScoped<EmailService>();
                builder.Services.AddHostedService<TcpSocketService>();
                builder.Services.AddMemoryCache();

                var app = builder.Build();

                app.UseRouting();
                app.MapControllers();

                app.Run("http://0.0.0.0:5000");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "A aplicação encerrou inesperadamente.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
