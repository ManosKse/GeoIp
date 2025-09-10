using GeoIpProject.Clients.Interfaces;
using GeoIpProject.Services.Interfaces;

namespace GeoIpProject.Services.Extensions
{
    internal static partial class Mapper
    {
        internal static IpLookupResponse ToIpLookupResponse(this GeoIpResponse input)
        {
            return new IpLookupResponse
            {
                CountryCode = input.Data.Location.Country.CountryCode,
                CountryName = input.Data.Location.Country.CountryName,
                Ip = input.Data.Ip,
                Latitude = input.Data.Location.Latitude,
                Longitude = input.Data.Location.Longitude,
                TimeZone = input.Data.TimeZone.Id
            };
        }
    }
}
