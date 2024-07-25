using GeocodingService.Configuration;
using GeocodingService.Formatters;
using Serilog;

namespace GeocodingService;

public class Program
{
    public static void Main(string[] args)
    {
        // Build configuration
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        // Bind configuration settings
        var loggingSettings = new LoggingSettings();
        configuration.GetSection("Logging").Bind(loggingSettings);

        var geocodingSettings = new GeocodingSettings();
        configuration.GetSection("Geocoding").Bind(geocodingSettings);

        var cacheSettings = new CacheSettings();
        configuration.GetSection("Cache").Bind(cacheSettings);

        // Configure Serilog
        var loggerConfig = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext();

        // Configure log sinks based on settings
        switch (loggingSettings.LogTo)
        {
            case 1:
                loggerConfig = loggerConfig.WriteTo.Console(new CustomLogFormatter(loggingSettings.LogFormat));
                break;
            case 2:
                loggerConfig = loggerConfig.WriteTo.File(new CustomLogFormatter(loggingSettings.LogFormat), loggingSettings.LogFilePath);
                break;
            case 3:
                if (string.IsNullOrEmpty(loggingSettings.ElkEndpoint))
                {
                    throw new ArgumentNullException("Logging:ElkEndpoint cannot be null or empty.");
                }
                loggerConfig = loggerConfig.WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri(loggingSettings.ElkEndpoint))
                {
                    AutoRegisterTemplate = true,
                });
                break;
            default:
                throw new ArgumentException("Invalid LogTo setting");
        }

        Log.Logger = loggerConfig.CreateLogger();

        try
        {
            Log.Information("Starting web host");
            CreateHostBuilder(args).Build().Run();
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "Host terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog() // Use Serilog for logging
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
