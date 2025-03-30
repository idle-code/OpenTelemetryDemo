using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = FunctionsApplication.CreateBuilder(args);
builder.ConfigureFunctionsWebApplication();
builder.Configuration.AddEnvironmentVariables();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

var app = builder.Build();

// TODO: Check how RabbitMq connection string can be passed via environment variables
var rabbitmqConnectionString = builder.Configuration.GetConnectionString("RabbitMq");
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("RabbitMq connection string: {RabbitMqConnectionString}", rabbitmqConnectionString);
Console.WriteLine("RabbitMq connection string: {0}", rabbitmqConnectionString);

app.Run();
