namespace FunctionsWorker;

public record SmtpOptions
{
    public required string FromAddress { get; init; }
    public required string Password { get; init; }
    public required string SmtpServer { get; init; }
}
