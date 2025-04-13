using Exception = System.Exception;

namespace WebAPI.Telemetry;

public class ExceptionLoggingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionLoggingMiddleware> _logger;

    public ExceptionLoggingMiddleware(ILogger<ExceptionLoggingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");
        }
    }
}
