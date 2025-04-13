using System.Collections.Specialized;
using System.Diagnostics;
using System.Web;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;

namespace WebAPI.Telemetry;

#region query-context-middleware
public class ContextFromQueryMiddleware : IMiddleware
{
    private static readonly TextMapPropagator Propagator = Propagators.DefaultTextMapPropagator;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        ExtractActivityContextFromUrl(context);
        await next(context);
    }

    private void ExtractActivityContextFromUrl(HttpContext context)
    {
        var activity = Activity.Current;
        if (activity is null || !context.Request.QueryString.HasValue)
        {
            return;
        }

        var queryParams = HttpUtility.ParseQueryString(context.Request.QueryString.Value);
        var parentContext = Propagator.Extract(default, queryParams, ExtractContextTagsFromQueryParams);
        Baggage.Current = parentContext.Baggage;

        activity.AddLink(new ActivityLink(parentContext.ActivityContext));
    }

    private static IEnumerable<string>? ExtractContextTagsFromQueryParams(NameValueCollection queryParams, string key)
    {
        var value = queryParams[$"_{key}"];
        return value is null ? null : [value];
    }
}
#endregion
