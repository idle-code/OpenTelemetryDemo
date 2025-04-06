using System.Collections.Specialized;
using System.Diagnostics;
using System.Web;
using Microsoft.AspNetCore.Http.Extensions;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;

namespace WebAPI;

public class ContextRetrievingMiddleware : IMiddleware
{
    private static readonly ActivitySource ActivitySource = new(typeof(ContextRetrievingMiddleware).FullName!);
    private static readonly TextMapPropagator Propagator = Propagators.DefaultTextMapPropagator;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        ExtractActivityContextFromUrl(context);

        await next(context);
    }

    private void ExtractActivityContextFromUrl(HttpContext context)
    {
        var activity = Activity.Current;
        if (activity is null)
        {
            return;
        }

        if (!context.Request.QueryString.HasValue)
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
        var value = queryParams[key];
        if (value is null)
        {
            return null;
        }

        return [value];
    }
}
