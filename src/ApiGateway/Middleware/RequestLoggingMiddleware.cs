using System.Diagnostics;

namespace ApiGateway.Middleware;

public class RequestLoggingMiddleware
{
    public const string CorrelationHeader = "X-Correlation-Id";

    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers[CorrelationHeader].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(correlationId))
        {
            correlationId = Guid.NewGuid().ToString("N");
        }

        context.Request.Headers[CorrelationHeader] = correlationId;
        context.Response.OnStarting(() =>
        {
            context.Response.Headers[CorrelationHeader] = correlationId;
            return Task.CompletedTask;
        });

        var cronometro = Stopwatch.StartNew();
        _logger.LogInformation("--> [{CorrelationId}] {Metodo} {Ruta}{Query} desde {Ip}",
            correlationId,
            context.Request.Method,
            context.Request.Path,
            context.Request.QueryString,
            context.Connection.RemoteIpAddress);

        try
        {
            await _next(context);
        }
        finally
        {
            cronometro.Stop();
            _logger.LogInformation("<-- [{CorrelationId}] {Metodo} {Ruta} => {Estado} en {Ms} ms",
                correlationId,
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                cronometro.ElapsedMilliseconds);
        }
    }
}
