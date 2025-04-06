using MediatR;
using Microsoft.EntityFrameworkCore;
using WebAPI.Model;

namespace WebAPI.Handlers;

public record ConfirmToken(string Token) : IRequest<bool>;

internal sealed class ConfirmTokenHandler : IRequestHandler<ConfirmToken, bool>
{
    private readonly ILogger<ConfirmTokenHandler> _logger;
    private readonly TheButtonDbContext _dbContext;
    private readonly TimeProvider _timeProvider;

    public ConfirmTokenHandler(ILogger<ConfirmTokenHandler> logger, TheButtonDbContext dbContext, TimeProvider timeProvider)
    {
        _logger = logger;
        _dbContext = dbContext;
        _timeProvider = timeProvider;
    }

    public async Task<bool> Handle(ConfirmToken request, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Validating token");
        var token = await _dbContext.BonusTokens.SingleOrDefaultAsync(t => t.Token == request.Token, cancellationToken);
        if (token is null)
        {
            _logger.LogWarning("Token not found");
            return false;
        }

        _dbContext.BonusTokens.Remove(token);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var currentTime = _timeProvider.GetUtcNow();
        if (token.ValidUntil < currentTime)
        {
            _logger.LogWarning("Token already expired");

            return false;
        }

        _logger.LogInformation("Token verified successfully");
        return true;
    }
}
