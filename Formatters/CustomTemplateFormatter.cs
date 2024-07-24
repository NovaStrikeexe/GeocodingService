using Serilog.Events;
using Serilog.Formatting;

namespace GeocodingService.Formatters;

public class CustomTemplateFormatter(string template) : ITextFormatter
{
    public void Format(LogEvent logEvent, TextWriter output)
    {
        var templateValues = new Dictionary<string, string>
        {
            { "Timestamp", logEvent.Timestamp.UtcDateTime.ToString("o") },
            { "Level", logEvent.Level.ToString() },
            { "Message", logEvent.RenderMessage() }
        };

        var formattedMessage = templateValues.Aggregate(template, (current, kvp) => current.Replace($"{{{kvp.Key}}}", kvp.Value));

        output.WriteLine(formattedMessage);
    }
}