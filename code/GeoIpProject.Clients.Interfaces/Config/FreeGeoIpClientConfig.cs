namespace GeoIpProject.Clients.Interfaces
{
    public class FreeGeoIpClientConfig
    {
        public const string Name = "FreeGeoIp-Client-Config";

        /// <summary>
        /// FreeGeoIp API base url. Example https://freegeoip.app/
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// FreeGeoIp API base bath. Example api/v1/
        /// </summary>
        public string BasePath { get; set; }

        /// <summary>
        /// Combined BaseUrl and BasePath https://freegeoip.app/json/
        /// </summary>
        public string ServicePath => $"{BaseUrl}{BasePath}";

        /// <summary>
        /// HttpClient request times out in minutes
        /// </summary>
        public int TimeoutMinutes { get; set; }

        /// <summary>
        /// HttpClient handler lifetime in minutes
        /// </summary>
        public int HandlerLifetimeMinutes { get; set; }
    }
}
