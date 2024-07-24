namespace GeocodingService.Models;

public class LoggingSettings
{
    public LogLevelSettings LogLevel { get; set; }
    public string LogFormat { get; set; }
    public string ElkEndpoint { get; set; }
}