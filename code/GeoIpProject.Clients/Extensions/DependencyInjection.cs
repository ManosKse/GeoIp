using GeoIpProject.Clients.Interfaces;
using GeoIpProject.Clients.Interfaces.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

namespace GeoIpProject.Clients
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjection
    {
        public static IServiceCollection AddFreeGeoIpClient(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddHttpClientFreeGeoIpClient(configuration)
                           .AddTransient<IFreeGeoIpClient, FreeGeoIpClient>()
                           .AddTransient<IBatchRepository, BatchRepository>()
                           .AddTransient<IIpLookupRepository, IpLookupRepository>();
        }

        public static IServiceCollection AddHostedService(this IServiceCollection services, IConfiguration configuration)
        {
            return services.Configure<WorkerOptions>(configuration.GetSection(WorkerOptions.Name))
                           .AddHostedService<IpLookupWorker>();
        }

        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            return services.Configure<DatabaseSettings>(configuration.GetSection(DatabaseSettings.Name))
                           .AddDbContext<AppDbContext>((serviceProvider, options) => options.UseSqlServer(serviceProvider.GetRequiredService<IOptions<DatabaseSettings>>().Value.ConnectionString));
        }

        private static IServiceCollection AddHttpClientFreeGeoIpClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FreeGeoIpClientConfig>(configuration.GetSection(FreeGeoIpClientConfig.Name));
            var config = configuration.GetSection(FreeGeoIpClientConfig.Name).Get<FreeGeoIpClientConfig>();

            services.AddHttpClient(FreeGeoIpClient.HTTP_CLIENT_NAME, httpClient =>
            {
                ConfigureHttpClient(httpClient, config);
            })
                    .SetHandlerLifetime(TimeSpan.FromMinutes(config.HandlerLifetimeMinutes));
            return services;
        }

        private static void ConfigureHttpClient(HttpClient httpClient, FreeGeoIpClientConfig config)
        {
            httpClient.BaseAddress = new Uri(config.BaseUrl);
            httpClient.Timeout = TimeSpan.FromMinutes(config.TimeoutMinutes);
        }
    }
}
