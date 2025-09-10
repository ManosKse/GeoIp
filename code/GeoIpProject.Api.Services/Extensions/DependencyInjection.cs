using GeoIpProject.Api.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace GeoIpProject.Api.Services
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjection
    {
        public static IServiceCollection AddGeoIpApiServices(this IServiceCollection services)
        {
            return services.AddTransient<IGeoIpProjectService, GeoIpProjectService>();
        }
    }
}