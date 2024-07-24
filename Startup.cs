using GeocodingService.Middleware;
using GeocodingService.Models;
using GeocodingService.Services;
using Microsoft.OpenApi.Models;

namespace GeocodingService;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<AppSettings>(Configuration);

        services.AddControllers();

        services.AddHttpClient<Services.GeocodingService>();
        services.AddHttpClient<ReverseGeocodingService>();

        services.AddMemoryCache();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Geocoding API",
                Version = "v1",
                Description = "API for Geocoding and Reverse Geocoding",
                Contact = new OpenApiContact
                {
                    Name = "Your Name",
                    Email = "your.email@example.com"
                }
            });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseMiddleware<ErrorHandlingMiddleware>();

        app.UseRouting();

        app.UseSwagger();

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Geocoding API V1");
            c.RoutePrefix = string.Empty;
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}