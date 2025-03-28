using MediatR;
using Microsoft.EntityFrameworkCore;
using WebAPI.Model;

namespace WebAPI;

public record IncrementNamedCounter(string CounterId, int Delta) : IRequest<int>;

internal class IncrementNamedCounterHandler : IRequestHandler<IncrementNamedCounter, int>
{
    private readonly ILogger<IncrementNamedCounterHandler> _logger;
    private readonly TheButtonDbContext _dbContext;

    public IncrementNamedCounterHandler(ILogger<IncrementNamedCounterHandler> logger, TheButtonDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<int> Handle(IncrementNamedCounter request, CancellationToken cancellationToken)
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
        counter.Value += request.Delta;
        await _dbContext.SaveChangesAsync(cancellationToken);

        var threshold = 10;
        if (counter.Value > threshold)
        {
            _logger.LogWarning("Counting threshold {ThreasholdValue} reached, notifying authorities", threshold);
            // TODO: Send RabbitMq message
            // Make it a cookie-clicker clone?
        }

        return counter.Value;
    }
}
