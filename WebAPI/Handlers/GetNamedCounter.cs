using MediatR;
using Microsoft.EntityFrameworkCore;
using WebAPI.Model;

namespace WebAPI.Handlers;

internal record GetNamedCounter(string CounterId) : IRequest<NamedCounter?>;

internal sealed class GetNamedCounterHandler : IRequestHandler<GetNamedCounter, NamedCounter?>
{
    private readonly ILogger<GetNamedCounterHandler> _logger;
    private readonly TheButtonDbContext _dbContext;

    public GetNamedCounterHandler(ILogger<GetNamedCounterHandler> logger, TheButtonDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<NamedCounter?> Handle(GetNamedCounter request, CancellationToken cancellationToken)
    {
        using var _ = _logger.BeginScope(new Dictionary<string, object>
        {
            { "CounterId", request.CounterId }
        });

        var counter = await _dbContext.NamedCounters.SingleOrDefaultAsync(counter => counter.Id == request.CounterId, cancellationToken);
        if (counter is null)
        {
            _logger.LogInformation("No existing counter found");
            return null;
        }

        return counter;
    }
}
