using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace FunctionsWorker;

public record CounterDetailsDto
{
    public string Id { get; init; }
    public int Delta { get; init; }
}

public class IncrementCounterFunction
{
    private readonly ILogger<IncrementCounterFunction> _logger;

    public IncrementCounterFunction(ILogger<IncrementCounterFunction> logger)
    {
        _logger = logger;
    }

    [Function(nameof(IncrementCounterHttpTrigger))]
    public async Task<HttpResponseData> IncrementCounterHttpTrigger(
        [HttpTrigger(AuthorizationLevel.Anonymous, methods: "post", Route = "counter")] HttpRequestData req,
        [FromBody] CounterDetailsDto counterDetails,
        CancellationToken cancellationToken)
    {
        using var _ = _logger.BeginScope(new Dictionary<string, object> { ["CounterId"] = counterDetails.Id });
        _logger.LogInformation("Incrementing counter by {Delta}", counterDetails.Delta);

        // TODO: increment counter in the db

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(123, cancellationToken);
        return response;
    }
}
