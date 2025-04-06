using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;

namespace FunctionsWorker;

public record ThresholdReachedMessage(string CounterId, int Threshold, string BonusToken);

public class NotificationFunction
{
    private const string ThresholdsQueueName = "thresholds";
    private static readonly ActivitySource ActivitySource = new(typeof(NotificationFunction).FullName!);
    private static readonly TextMapPropagator Propagator = Propagators.DefaultTextMapPropagator;

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
        Dictionary<string, JsonNode?> basicProperties,
        FunctionContext context)
    {
        using var activity = ExtractTracingContext(basicProperties);

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

    private Activity? ExtractTracingContext(Dictionary<string, JsonNode?> context)
    {
        var headersObject = (JsonObject)context["Headers"]!;
        var parentContext = Propagator.Extract(default, headersObject, ExtractContextTags);
        Baggage.Current = parentContext.Baggage;

        return ActivitySource.StartActivity("RabbitMQ Receiver", ActivityKind.Consumer, parentContext.ActivityContext);
    }

    private IEnumerable<string> ExtractContextTags(JsonObject? messageHeaders, string headerKey)
    {
        try
        {
            if (messageHeaders?.TryGetPropertyValue(headerKey, out var node) == true)
            {
                var base64HeaderValue = node?.GetValue<string>();
                if (base64HeaderValue is not null)
                {
                    var headerValue = Encoding.UTF8.GetString(Convert.FromBase64String(base64HeaderValue));
                    return [headerValue];
                }
            }

            return [];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not deserialize context header {HeaderKey}", headerKey);
            return []; // In case of failure during injection/extraction we should not throw
        }
    }

    private async ValueTask SendNotification(ThresholdReachedMessage message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received {@Message}", message);
        var tokenUrl = $"http://localhost:8081/confirm?token={UrlEncoder.Default.Encode(message.BonusToken)}";
        await SendEmail(
            toAddress: "p.z.idlecode@gmail.com",
            subject: "Threshold reached!",
            body: $"""
                  <h1>Congratulations!</h1>
                  <div>
                  You've reached {message.Threshold} on {message.CounterId} counter, click the link below to grab additional points:<br/>
                  <a href="{tokenUrl}">{tokenUrl}</a>
                  </div>
                  """,
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
        message.IsBodyHtml = true;
        message.Subject = subject;
        message.Body = body;
        await smtp.SendMailAsync(message, cancellationToken);
    }
}
