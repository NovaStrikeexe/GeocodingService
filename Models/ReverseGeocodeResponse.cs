namespace GeocodingService.Models;

public class ReverseGeocodeResponse
{
    public string Value { get; set; }
    public string UnrestrictedValue { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}