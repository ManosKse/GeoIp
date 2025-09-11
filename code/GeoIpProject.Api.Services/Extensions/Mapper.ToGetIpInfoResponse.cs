using GeoIpProject.Api.Services.Interfaces;
using GeoIpProject.Services.Interfaces;

namespace GeoIpProject.Api.Services
{
    internal static partial class Mapper
    {
        internal static GetIpInfoResponse ToGetIpInfoResponse(this IpLookupResponse input)
        {
            return new GetIpInfoResponse
            {
                Ip = input.Ip,
                CountryCode = input.CountryCode,
                CountryName = input.CountryName,
                Latitude = input.Latitude,
                Longitude = input.Longitude,
                TimeZone = input.TimeZone
            };
        }
    }
}
