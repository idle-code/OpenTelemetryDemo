using System.Text;
using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using WebAPI.Model;

namespace WebAPI;

internal record IncrementNamedCounter(string CounterId, int Delta) : IRequest<NamedCounter>;

internal record ThresholdReachedMessage(string CounterId, int Threshold);

internal class IncrementNamedCounterHandler : IRequestHandler<IncrementNamedCounter, NamedCounter>
{
    const int Threshold = 10;
    private const string ThresholdsQueueName = "thresholds";

    private readonly ILogger<IncrementNamedCounterHandler> _logger;
    private readonly TheButtonDbContext _dbContext;
    private readonly IConnectionFactory _rabbitConnectionFactory;

    public IncrementNamedCounterHandler(ILogger<IncrementNamedCounterHandler> logger, TheButtonDbContext dbContext, IConnectionFactory rabbitConnectionFactory)
    {
        _logger = logger;
        _dbContext = dbContext;
        _rabbitConnectionFactory = rabbitConnectionFactory;
    }

    public async Task<NamedCounter> Handle(IncrementNamedCounter request, CancellationToken cancellationToken)
    {
        using var _ = _logger.BeginScope(new Dictionary<string, object>
        {
            { "CounterId", request.CounterId }
        });

        var counter = await _dbContext.NamedCounters.SingleOrDefaultAsync(counter => counter.Id == request.CounterId, cancellationToken);
        if (counter is null)
        {
            _logger.LogInformation("No existing counter found - creating a new one");
            counter = new NamedCounter
            {
                Id = request.CounterId
            };
            _dbContext.NamedCounters.Add(counter);
        }

        _logger.LogInformation("Incrementing {CounterId} counter by {Delta}", request.CounterId, request.Delta);
        var oldValue = counter.Value;
        counter.Value += request.Delta;
        await _dbContext.SaveChangesAsync(cancellationToken);

        var reachedThreshold = GetThresholdReached(oldValue, counter.Value);
        if (reachedThreshold is not null)
        {
            _logger.LogWarning("Counting threshold {ThresholdValue} reached - notifying authorities", reachedThreshold);
            var message = new ThresholdReachedMessage(counter.Id, reachedThreshold.Value);
            await PublishMessage(message, cancellationToken);
        }

        return counter;
    }

    private int? GetThresholdReached(int oldValue, int newValue)
    {
        var threshold = (newValue / Threshold) * Threshold;
        _logger.LogDebug("Checking if {NewVale} reached threshold {Threshold}", newValue, threshold);
        if (oldValue >= threshold || newValue < threshold)
        {
            return null;
        }

        return threshold;
    }

    private async ValueTask PublishMessage(ThresholdReachedMessage message, CancellationToken cancellationToken)
    {
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
            messageBytes,
            cancellationToken: cancellationToken);
    }
}
