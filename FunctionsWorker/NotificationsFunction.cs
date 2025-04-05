using System.Net;
using System.Net.Mail;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FunctionsWorker;

public record ThresholdReachedMessage(string CounterId, int Threshold);

public class NotificationFunction
{
    private const string ThresholdsQueueName = "thresholds";

    private readonly ILogger<NotificationFunction> _logger;
    private readonly SmtpOptions _smtpOptions;

    public NotificationFunction(ILogger<NotificationFunction> logger, IOptions<SmtpOptions> smtpOptions)
    {
        _logger = logger;
        _smtpOptions = smtpOptions.Value;
    }

    [Function(nameof(SendThresholdReachedNotification))]
    public async ValueTask SendThresholdReachedNotification(
        [RabbitMQTrigger(ThresholdsQueueName, ConnectionStringSetting = "RabbitMQ")]
        string messageString,
        FunctionContext context)
    {
        var message = JsonSerializer.Deserialize<ThresholdReachedMessage>(messageString);
        await SendNotification(message!, context.CancellationToken);
    }

    [Function(nameof(SendThresholdReachedNotificationHttp))]
    public async ValueTask SendThresholdReachedNotificationHttp(
        [HttpTrigger(AuthorizationLevel.Anonymous, ["POST"], Route = "send-threshold-notification")]
        [FromBody] ThresholdReachedMessage message,
        FunctionContext context)
    {
        await SendNotification(message, context.CancellationToken);
    }

    private async ValueTask SendNotification(ThresholdReachedMessage message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received {@Message}", message);
        await SendEmail(
            toAddress: "p.z.idlecode@gmail.com",
            subject: "Threshold reached!",
            body: "Congrats!",
            cancellationToken: cancellationToken);
    }

    private async ValueTask SendEmail(string toAddress, string subject, string body, CancellationToken cancellationToken)
    {
        using var _ = _logger.BeginScope(new Dictionary<string, object> { { "Subject", subject } });
        _logger.LogInformation("Sending email to {ToAddress}", toAddress);

        var smtp = new SmtpClient
        {
            Host = _smtpOptions.Server,
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_smtpOptions.FromAddress, _smtpOptions.Password)
        };

        using var message = new MailMessage(_smtpOptions.FromAddress, toAddress);
        message.Subject = subject;
        message.Body = body;
        await smtp.SendMailAsync(message, cancellationToken);
    }
}
