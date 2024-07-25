using Microsoft.AspNetCore.Mvc;

namespace GeocodingService.Handlers.Interfaces
{
    public interface IHandler<TRequest>
    {
        Task<IActionResult> HandleAsync(TRequest request);
    }
}
