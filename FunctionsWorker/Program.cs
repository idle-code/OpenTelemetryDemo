using Azure.Monitor.OpenTelemetry.Exporter;
using FunctionsWorker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);
builder.ConfigureFunctionsWebApplication();
builder.Configuration.AddEnvironmentVariables();

var smtpOptions = builder.Configuration.GetSection(nameof(SmtpOptions)).Get<SmtpOptions>();
builder.Services.AddSingleton(smtpOptions!);

builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection(nameof(SmtpOptions)));

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

#region opentelemetry-setup

var otel = builder.Services.AddOpenTelemetry();
otel.WithLogging()
    .WithTracing()
    .WithMetrics();

var applicationInsightsConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
if (!string.IsNullOrEmpty(applicationInsightsConnectionString))
{
    otel.UseAzureMonitorExporter(options =>
    {
        options.ConnectionString = applicationInsightsConnectionString;
    });
}

#endregion

var app = builder.Build();

app.Run();
