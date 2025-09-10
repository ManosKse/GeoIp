namespace GeoIpProject.Api.Services.Interfaces
{
    public class GetIpInfoResponse
    {
        public string Ip { get; set; }

        public string CountryCode { get; set; }

        public string CountryName { get; set; }

        public string TimeZone { get; set; }

        public double Latitude { get; set; }

        public double Longtitude { get; set; }
    }
}
