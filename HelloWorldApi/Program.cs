using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.Extensibility;
using Scalar.AspNetCore;
using Serilog;
using Serilog.AspNetCore;
using Serilog.Sinks.ApplicationInsights;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddApplicationInsightsTelemetry();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//add serilog
builder.Host.UseSerilog((context, services, configuration) => {
    
    configuration.WriteTo.ApplicationInsights(
        TelemetryConfiguration.CreateDefault(), TelemetryConverter.Traces
        );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
