using System.Text.Json.Serialization;

namespace GeoIpProject.Clients.Interfaces
{
    public class GeoIpResponse
    {
        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    public class Data
    {
        [JsonPropertyName("ip")]
        public string Ip { get; set; }

        [JsonPropertyName("location")]
        public Location Location { get; set; }

        [JsonPropertyName("timezone")]
        public TimeZone TimeZone { get; set; }
    }

    public class Location
    {
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("country")]
        public Country Country { get; set; }
    }

    public class Country
    {
        [JsonPropertyName("alpha2")]
        public string CountryCode { get; set; }

        [JsonPropertyName("name")]
        public string CountryName { get; set; }
    }

    public class TimeZone
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
