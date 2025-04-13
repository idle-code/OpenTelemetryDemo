using System.Diagnostics;
using OpenTelemetry;

namespace WebAPI.Telemetry;

#region query-processor
public class QueryFilteringProcessor : BaseProcessor<Activity>
{
    public override void OnEnd(Activity data)
    {
        var commandText = data.GetTagItem("db.statement");
        if (commandText is not null)
        {
            var query = (string)commandText;
            if (query.StartsWith("SELECT") && data.Duration < TimeSpan.FromMilliseconds(30))
            {
                data.ActivityTraceFlags &= ~ActivityTraceFlags.Recorded;
            }
        }
    }
}
#endregion
