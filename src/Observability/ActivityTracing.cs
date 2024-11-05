using System.Diagnostics;

namespace Observability;

public static class ActivityTracing
{
    public static ActivitySource Source { get; private set; } = new ActivitySource("Observability", "1.0.0");
}