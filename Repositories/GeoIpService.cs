using ABC.Models.DTO;
using MaxMind.GeoIP2;
using Microsoft.Extensions.Hosting;

namespace ABC.Repositories
{
    public class GeoIpService : IGeoIpService
    {
        private readonly DatabaseReader _reader;

        public GeoIpService(IWebHostEnvironment env)
        {
            var dbPath = Path.Combine(
                env.ContentRootPath,
                "App_Data",
                "GeoLite2-Country.mmdb"
            );

            if (!File.Exists(dbPath))
                throw new FileNotFoundException("GeoLite2 DB not found", dbPath);

            _reader = new DatabaseReader(dbPath);
        }

        public Task<string> GetCountryCodeAsync(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip))
                return Task.FromResult<string>(null);

            try
            {
                var response = _reader.Country(ip);
                return Task.FromResult(response?.Country?.IsoCode); // e.g. "IN"
            }
            catch
            {
                // Invalid IP / local IP / IPv6 loopback
                return Task.FromResult<string>(null);
            }
        }
    }

}
