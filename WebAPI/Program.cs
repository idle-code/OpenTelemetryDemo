using System.Security.Authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using WebAPI;
using WebAPI.Model;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(cors => cors.AddDefaultPolicy(policy =>
{
    policy.AllowAnyHeader();
    policy.AllowAnyMethod();
    policy.AllowAnyOrigin();
}));

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblyContaining<IncrementNamedCounterHandler>();
});

builder.Services.AddSingleton<IConnectionFactory, ConnectionFactory>(services =>
{
    var connectionString = services.GetRequiredService<IConfigurationManager>().GetConnectionString("RabbitMq")!;

    return new ConnectionFactory
    {
        Endpoint = new AmqpTcpEndpoint(new Uri(connectionString)),
        // UserName = null,
        // Password = null,
        // Uri = null,
        // ClientProvidedName = null
    };
});

var connectionString = builder.Configuration.GetConnectionString("TheButton")!;
builder.Services.AddDbContext<TheButtonDbContext>(opt => opt.UseNpgsql(connectionString));

var app = builder.Build();

{
    using var scope = app.Services.CreateScope();
    scope.ServiceProvider.GetRequiredService<TheButtonDbContext>().Database.Migrate();
}

app.UseCors();

app.MapGet("/counter/{id}", async (
    [FromRoute] string id,
    IMediator mediator,
    CancellationToken cancellationToken) => await mediator.Send(new GetNamedCounter(id), cancellationToken));

app.MapPost("/counter/{id}/increment", async (
    [FromRoute] string id,
    [FromBody] int byValue,
    IMediator mediator,
    CancellationToken cancellationToken) => await mediator.Send(new IncrementNamedCounter(id, byValue), cancellationToken));

app.Run();
