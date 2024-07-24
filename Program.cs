using Serilog;
using GeocodingService.Models;

namespace GeocodingService;

public class Program
{
    public static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var loggingSettings = new LoggingSettings();
        configuration.GetSection("Logging").Bind(loggingSettings);

        var geocodingSettings = new GeocodingSettings();
        configuration.GetSection("Geocoding").Bind(geocodingSettings);

        var cacheSettings = new CacheSettings();
        configuration.GetSection("Cache").Bind(cacheSettings);

        if (string.IsNullOrEmpty(loggingSettings.ElkEndpoint))
        {
            throw new ArgumentNullException("Logging:ElkEndpoint cannot be null or empty.");
        }

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console(new Serilog.Formatting.Json.JsonFormatter())
            .WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri(loggingSettings.ElkEndpoint))
            {
                AutoRegisterTemplate = true,
            })
            .CreateLogger();

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
            .UseSerilog() 
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}