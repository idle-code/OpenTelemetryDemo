using FunctionsWorker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

var connectionString = builder.Configuration.GetConnectionString("TheButton")!;
builder.Services.AddDbContext<TheButtonDbContext>(opt => opt.UseNpgsql(connectionString));

var app = builder.Build();

{
    using var scope = app.Services.CreateScope();
    scope.ServiceProvider.GetRequiredService<TheButtonDbContext>().Database.Migrate();
}

app.Run();
