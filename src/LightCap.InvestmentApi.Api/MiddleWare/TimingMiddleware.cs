using System.Diagnostics;

namespace CustOps.Api.Middleware;

public class TimingMiddleware(
    RequestDelegate next,
    ILogger<TimingMiddleware> logger)
{
    private static readonly string Separator = new('*', 110);
    private readonly Stopwatch _stopwatch = new();

    public async Task InvokeAsync(HttpContext context)
    {
        _stopwatch.Restart();
        await next(context);
        _stopwatch.Stop();

        var requestPath = context.Request.Path;
        var statusCode = context.Response.StatusCode;
        var elapsedMs = _stopwatch.ElapsedMilliseconds;

        logger.LogInformation("""
                              {Separator}
                              Request: {RequestPath}

                              Status Code: {StatusCode}

                              Elapsed Time: {ElapsedMilliseconds} ms
                              {Separator}
                              """,
            Separator, requestPath, statusCode, elapsedMs, Separator);
    }
}