namespace WebAPI.Model;

public record BonusToken
{
    public required string Token { get; init; }
    public required string CounterId { get; init; }

    public DateTimeOffset ValidUntil { get; set; }
}
