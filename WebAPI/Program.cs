using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(cors => cors.AddDefaultPolicy(policy =>
{
    policy.AllowAnyHeader();
    policy.AllowAnyMethod();
    policy.AllowAnyOrigin();
}));

var app = builder.Build();

app.UseCors();

var counters = new Dictionary<string, int>();

app.MapPost("/counter/{id}/increment", async (string id, [FromBody] int byValue) =>
{
    var currentValue = counters.GetValueOrDefault(id, 0);
    counters[id] = currentValue + byValue;
    Console.WriteLine($"Incrementing {id} ({currentValue}) by {byValue}");
    return counters[id];
});

app.Run();
