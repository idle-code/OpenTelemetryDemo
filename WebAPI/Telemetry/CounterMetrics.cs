using System.Diagnostics.Metrics;

namespace WebAPI.Telemetry;

#region counter-metrics
public class CounterMetrics
{
    private readonly Counter<int> _counterIncrements;

    public CounterMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create("WebAPI.counter");
        _counterIncrements = meter.CreateCounter<int>("WebAPI.counter.increments");
    }

    public void CounterIncrement(string counterName, int delta)
    {
        _counterIncrements.Add(delta, new[] { new KeyValuePair<string, object?>("counter_name", counterName) });
    }
}
#endregion
