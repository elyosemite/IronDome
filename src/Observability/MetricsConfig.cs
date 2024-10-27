using System.Diagnostics.Metrics;

namespace Observability;

public static class MetricsConfig
{
    public static readonly string MetricName = "IronDome";
    private static Meter _meter = new Meter(MetricName, "1.0");

    public static Counter<long> TotalRequestCounter = _meter.CreateCounter<long>("requests_total");
    public static UpDownCounter<long> ActiveRequestUpDownCounter = _meter.CreateUpDownCounter<long>("active_requests");
    public static Histogram<double> RequestDurationHistogram = _meter.CreateHistogram<double>("request_duration_ms");
    public static ObservableGauge<long> CustomGauge = _meter.CreateObservableGauge("memory_usage", () => GC.GetTotalMemory(false));
}