using Azure.Monitor.OpenTelemetry.Exporter;
using FunctionsWorker;
using FunctionsWorker.Telemetry;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = FunctionsApplication.CreateBuilder(args);
builder.ConfigureFunctionsWebApplication();
builder.Configuration.AddEnvironmentVariables();

var smtpOptions = builder.Configuration.GetSection(nameof(SmtpOptions)).Get<SmtpOptions>();
builder.Services.AddSingleton(smtpOptions!);

builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection(nameof(SmtpOptions)));

#region opentelemetry-setup
var otel = builder.Services.AddOpenTelemetry();
otel
    .ConfigureResource(resource => resource
        .AddService("FunctionsWorker"))
    .WithLogging()
    .WithTracing(tracing => tracing
        .AddHttpClientInstrumentation()
        .AddSource("FunctionsWorker.*")
        .AddProcessor<BaggageEnrichingProcessor>()
    )
    .WithMetrics(metrics => metrics
        .AddHttpClientInstrumentation()
        .AddProcessInstrumentation()
        .AddRuntimeInstrumentation()
        .AddMeter("FunctionsWorker.*"));
#endregion

var applicationInsightsConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
if (!string.IsNullOrEmpty(applicationInsightsConnectionString))
{
    otel.UseAzureMonitorExporter(options =>
    {
        options.ConnectionString = applicationInsightsConnectionString;
    });
}

var app = builder.Build();

app.Run();
