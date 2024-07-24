namespace GeocodingService.Models;

public class AppSettings
{
    public LoggingSettings Logging { get; set; }
    public GeocodingSettings Geocoding { get; set; }
    public CacheSettings Cache { get; set; }
}