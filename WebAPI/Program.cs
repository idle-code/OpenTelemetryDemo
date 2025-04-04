using Azure.Monitor.OpenTelemetry.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using WebAPI;
using WebAPI.Model;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(cors => cors.AddDefaultPolicy(policy =>
{
    policy.AllowAnyHeader();
    policy.AllowAnyMethod();
    policy.AllowAnyOrigin();
}));

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

var connectionString = builder.Configuration.GetConnectionString("TheButton")!;
builder.Services.AddDbContext<TheButtonDbContext>(opt => opt.UseNpgsql(connectionString));

#region opentelemetry-setup
var otel = builder.Services.AddOpenTelemetry();
otel
    .ConfigureResource(resource => resource
        .AddService("WebAPI"))
    .WithLogging()
    .WithTracing(tracing => tracing
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddSqlClientInstrumentation()
        .AddGrpcClientInstrumentation()
        .AddEntityFrameworkCoreInstrumentation()
        .AddSource("WebAPI.*"))
    .WithMetrics(metrics => metrics
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddSqlClientInstrumentation());

if (!string.IsNullOrEmpty(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
{
    otel.UseAzureMonitor();
}
#endregion

var app = builder.Build();

{
    using var scope = app.Services.CreateScope();
    scope.ServiceProvider.GetRequiredService<TheButtonDbContext>().Database.Migrate();
}

app.UseCors();

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

#endregion

app.Run();
