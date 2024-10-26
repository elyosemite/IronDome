using System.Diagnostics.Metrics;

namespace Observability;

public static class MetricsConfig
{
    private static Meter _meter = new Meter("CustomMetrics", "1.0");

    public static Counter<long> CustomCounter1 = _meter.CreateCounter<long>("requests_total");
    public static Histogram<double> CustomHistogram = _meter.CreateHistogram<double>("request_duration_ms");
    public static UpDownCounter<long> CustomUpDownCounter = _meter.CreateUpDownCounter<long>("active_requests");
    public static ObservableGauge<long> CustomGauge = _meter.CreateObservableGauge("memory_usage", () => GC.GetTotalMemory(false));

}