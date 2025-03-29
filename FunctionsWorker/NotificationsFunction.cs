using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionsWorker;

internal record ThresholdReachedMessage(string CounterId, int Threshold);

public class NotificationFunction
{
    private const string ThresholdsQueueName = "thresholds";

    private readonly ILogger<NotificationFunction> _logger;

    public NotificationFunction(ILogger<NotificationFunction> logger)
    {
        _logger = logger;
    }

    [Function(nameof(SendThresholdReachedNotification))]
    public async Task SendThresholdReachedNotification(
        [RabbitMQTrigger(ThresholdsQueueName, ConnectionStringSetting = "RabbitMQ")]
        string messageString,
        FunctionContext context)
    {
        _logger.LogInformation("Received {Message}", messageString);
        var message = JsonSerializer.Deserialize<ThresholdReachedMessage>(messageString);
        // TODO: Do something with the message
    }
}
