namespace GeocodingService.Configuration;

public class LoggingSettings
{
    public LogLevelSettings LogLevel { get; set; }
    public string LogFormat { get; set; }
    public string ElkEndpoint { get; set; }
    public string LogFilePath { get; set; }
    public int LogTo { get; set; }
}