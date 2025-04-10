using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using MediatR;
using OpenTelemetry;

namespace WebAPI.Telemetry;

public class LoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    #region logging-behavior
    private static readonly ActivitySource ActivitySource
        = new(typeof(LoggingPipelineBehavior<TRequest, TResponse>).FullName!);

    public async Task<TResponse> Handle(
        TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestTypeName = typeof(TRequest).Name;
        using var activity = ActivitySource.StartActivity($"Handling {requestTypeName}", ActivityKind.Internal);

        var requestTags = ToKeyValuePairs(request);
        foreach (var (key, value) in requestTags)
        {
            activity?.SetTag(key, value);
        }

        Baggage.SetBaggage(requestTags);
        // TODO: Use baggage to populate traces

        return await next();
    }

    #endregion

    private List<KeyValuePair<string,string?>> ToKeyValuePairs(TRequest request)
    {
        var jsonRequest = (JsonObject)JsonNode.Parse(JsonSerializer.Serialize(request))!;
        return ToKeyValuePairs(jsonRequest).ToList();
    }

    private IEnumerable<KeyValuePair<string,string?>> ToKeyValuePairs(JsonObject jsonObject, string parentKey = "request")
    {
        foreach (var (key, node) in jsonObject)
        {
            if (node is null)
            {
                continue;
            }

            if (node.GetValueKind() == JsonValueKind.Object)
            {
                foreach (var kv in ToKeyValuePairs((JsonObject)node, key))
                {
                    yield return kv;
                }
            }
            else
            {
                yield return new KeyValuePair<string, string?>(
                    string.Join(".", parentKey, key),
                    node.ToString());
            }
        }
    }
}
