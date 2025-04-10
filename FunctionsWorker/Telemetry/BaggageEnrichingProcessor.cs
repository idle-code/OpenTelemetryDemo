using System.Diagnostics;
using OpenTelemetry;

namespace FunctionsWorker.Telemetry;

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
