using Azure.Monitor.OpenTelemetry.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Logs;
using RabbitMQ.Client;
using WebAPI;
using WebAPI.Model;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using WebAPI.Handlers;
using WebAPI.Telemetry;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(cors => cors.AddDefaultPolicy(policy =>
{
    policy.AllowAnyHeader();
    policy.AllowAnyMethod();
    policy.AllowAnyOrigin();
}));

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddMediatR(
    config =>
    {
        config.RegisterServicesFromAssemblyContaining<IncrementNamedCounterHandler>();
    })
    .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));

builder.Services.AddSingleton<IConnectionFactory, ConnectionFactory>(services =>
{
    var connectionString = services.GetRequiredService<IConfiguration>().GetConnectionString("RabbitMq")!;
    return new ConnectionFactory
    {
        Endpoint = new AmqpTcpEndpoint(new Uri(connectionString)),
    };
});
builder.Services.AddScoped<MessagePublisher>();
builder.Services.AddScoped<ContextRetrievingMiddleware>();
builder.Services.AddSingleton<CounterMetrics>();

var connectionString = builder.Configuration.GetConnectionString("TheButton")!;
builder.Services.AddDbContext<TheButtonDbContext>(opt => opt.UseNpgsql(connectionString));

#region opentelemetry-setup
var otel = builder.Services.AddOpenTelemetry();
otel
    .ConfigureResource(resource => resource
        .AddService("WebAPI"))
    .WithLogging(logging => logging
        .AddConsoleExporter())
    .WithTracing(tracing => tracing
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddSqlClientInstrumentation()
        .AddGrpcClientInstrumentation()
        .AddEntityFrameworkCoreInstrumentation()
        .AddSource("WebAPI.*")
        .AddProcessor<BaggageEnrichingProcessor>())
    .WithMetrics(metrics => metrics
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddSqlClientInstrumentation()
        .AddMeter("WebAPI.*"));
#endregion

if (!string.IsNullOrEmpty(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
{
    otel.UseAzureMonitor();
}


var app = builder.Build();

{
    using var scope = app.Services.CreateScope();
    scope.ServiceProvider.GetRequiredService<TheButtonDbContext>().Database.Migrate();
}

app.UseCors();

app.UseMiddleware<ContextRetrievingMiddleware>();

#region endpoints

app.MapGet("/counter/{id}", async (
        [FromRoute] string id,
        IMediator mediator,
        CancellationToken cancellationToken) =>
    {
        var counter = await mediator.Send(new GetNamedCounter(id), cancellationToken);
        return counter is null ? Results.NotFound() : Results.Ok(counter);
    })
    .Produces<NamedCounter?>();

app.MapPost("/counter/{id}/increment", async (
        [FromRoute] string id,
        [FromBody] int byValue,
        IMediator mediator,
        CancellationToken cancellationToken) =>
    {
        return await mediator.Send(new IncrementNamedCounter(id, byValue), cancellationToken);
    })
    .Produces<NamedCounter>();

app.MapGet("/confirm", async (
    [FromQuery] string token,
    IMediator mediator,
    CancellationToken cancellationToken) =>
{
    var validationSuccessful = await mediator.Send(new ConfirmToken(token), cancellationToken);
    return validationSuccessful ? Results.Accepted() : Results.BadRequest();
});

#endregion

app.Run();
