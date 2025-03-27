namespace WebAPI.Model;

public record NamedCounter
{
    public required string Id { get; init; }

    public int Value { get; set; }
}
