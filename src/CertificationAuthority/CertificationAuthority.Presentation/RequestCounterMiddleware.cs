using System.Diagnostics.Metrics;

namespace CertificationAuthority.Presentation;

public class RequestCounterMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Counter<long> _requestCounter;

    public RequestCounterMiddleware(RequestDelegate next, Meter meter)
    {
        _next = next;
        _requestCounter = meter.CreateCounter<long>("request_total");
    }

    public async Task Invoke(HttpContext context)
    {
        _requestCounter.Add(1);
        await _next(context);
    }
}