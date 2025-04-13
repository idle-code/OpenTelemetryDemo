using System.Diagnostics;
using System.Text;
using System.Text.Json;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using RabbitMQ.Client;

namespace WebAPI;

public record ThresholdReachedMessage(string CounterId, int Threshold, string BonusToken);

public class MessagePublisher
{
    private const string ThresholdsQueueName = "thresholds";
    private static readonly ActivitySource ActivitySource = new(typeof(MessagePublisher).FullName!);
    private static readonly TextMapPropagator Propagator = Propagators.DefaultTextMapPropagator;

    private readonly ILogger<MessagePublisher> _logger;
    private readonly IConnectionFactory _rabbitConnectionFactory;

    public MessagePublisher(ILogger<MessagePublisher> logger, IConnectionFactory rabbitConnectionFactory)
    {
        _logger = logger;
        _rabbitConnectionFactory = rabbitConnectionFactory;
    }

    #region publish-message
    public async ValueTask PublishMessage(ThresholdReachedMessage message, CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity("RabbitMQ Sender", ActivityKind.Producer);
        _logger.LogInformation("Sending message {@Message}", message);

        await using var rabbitConnection = await _rabbitConnectionFactory.CreateConnectionAsync(cancellationToken);
        await using var channel = await rabbitConnection.CreateChannelAsync(cancellationToken: cancellationToken);
        await channel.QueueDeclareAsync(
            queue: ThresholdsQueueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);

        var messageJson = JsonSerializer.Serialize(message);
        var messageBytes = Encoding.UTF8.GetBytes(messageJson);
        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: ThresholdsQueueName,
            mandatory: true,
            body: messageBytes,
            cancellationToken: cancellationToken);
    }
    #endregion
}
