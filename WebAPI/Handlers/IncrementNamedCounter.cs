using MediatR;
using Microsoft.EntityFrameworkCore;
using WebAPI.Model;
using WebAPI.Telemetry;

namespace WebAPI.Handlers;

internal record IncrementNamedCounter(string CounterId, int Delta) : IRequest<NamedCounter>;

internal class IncrementNamedCounterHandler : IRequestHandler<IncrementNamedCounter, NamedCounter>
{
    const int Threshold = 10;

    private readonly ILogger<IncrementNamedCounterHandler> _logger;
    private readonly TheButtonDbContext _dbContext;
    private readonly MessagePublisher _messagePublisher;
    private readonly TimeProvider _timeProvider;
    private readonly CounterMetrics _counterMetrics;

    public IncrementNamedCounterHandler(
        ILogger<IncrementNamedCounterHandler> logger,
        TheButtonDbContext dbContext,
        MessagePublisher messagePublisher,
        TimeProvider timeProvider,
        CounterMetrics counterMetrics)
    {
        _logger = logger;
        _dbContext = dbContext;
        _messagePublisher = messagePublisher;
        _timeProvider = timeProvider;
        _counterMetrics = counterMetrics;
    }

    #region handle-method
    public async Task<NamedCounter> Handle(IncrementNamedCounter request, CancellationToken cancellationToken)
    {
        using var _ = _logger.PushProperty("CounterId", request.CounterId);

        var counter = await _dbContext.NamedCounters.SingleOrDefaultAsync(counter => counter.Id == request.CounterId, cancellationToken);
        if (counter is null)
        {
            _logger.LogInformation("No existing counter found - creating a new one");
            counter = new NamedCounter { Id = request.CounterId };
            _dbContext.NamedCounters.Add(counter);
        }

        _logger.LogInformation("Incrementing {CounterId} counter by {Delta}", request.CounterId, request.Delta);
        var oldValue = counter.Value;
        counter.Value += request.Delta;
        _counterMetrics.CounterIncrement(counter.Id, request.Delta);
        #endregion

        var reachedThreshold = GetThresholdReached(oldValue, counter.Value);
        if (reachedThreshold is not null)
        {
            _logger.LogWarning("Threshold {ThresholdValue} reached", reachedThreshold);

            _logger.LogDebug("Generating bonus token");
            var bonusToken = GenerateBonusToken(counter.Id);

            var message = new ThresholdReachedMessage(counter.Id, reachedThreshold.Value, bonusToken);
            await _messagePublisher.PublishMessage(message, cancellationToken);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return counter;
    }

    private string GenerateBonusToken(string counterId)
    {
        var token = new BonusToken
        {
            Token = Guid.NewGuid().ToString("N"),
            CounterId = counterId,
            ValidUntil = _timeProvider.GetUtcNow().AddSeconds(30)
        };
        _dbContext.Add(token);
        return token.Token;
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
}
