using GeoIpProject.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace GeoIpProject.Services
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjection
    {
        public static IServiceCollection AddGeoIpProjectServices(this IServiceCollection services)
        {
            return services.AddTransient<IBatchService, BatchService>()
                           .AddTransient<IFreeGeoIpService, FreeGeoIpService>();
        }
    }
}
