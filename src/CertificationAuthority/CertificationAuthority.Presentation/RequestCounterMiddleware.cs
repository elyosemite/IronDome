using System.Diagnostics;
using Observability;

namespace CertificationAuthority.Presentation;

public class RequestCounterMiddleware
{
    private readonly RequestDelegate _next;

    public RequestCounterMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        MetricsConfig.TotalRequestCounter.Add(1);

        MetricsConfig.ActiveRequestUpDownCounter.Add(1);

        var stopWatch = Stopwatch.StartNew();
        try
        {
            await _next(context);
        }
        finally
        {
            stopWatch.Stop();

            MetricsConfig.RequestDurationHistogram.Record(stopWatch.ElapsedMilliseconds);

            MetricsConfig.ActiveRequestUpDownCounter.Add(-1);
        }
    }
}