using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Metrics;

namespace WebAPI.Telemetry;

#region baggage-processor
public class BaggageEnrichingProcessor : BaseProcessor<Activity>
{
    public override void OnStart(Activity data)
    {
        foreach (var (tagKey, tagValue) in Baggage.Current)
        {
            data.SetTag(tagKey, tagValue);
        }
    }
}
#endregion
