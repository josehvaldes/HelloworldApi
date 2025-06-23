using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.Extensibility;
using Scalar.AspNetCore;
using Serilog;
using Serilog.AspNetCore;
using Serilog.Sinks.ApplicationInsights;
using System.IO;
using System;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddApplicationInsightsTelemetry();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var log = new LoggerConfiguration().CreateLogger();

//add serilog
builder.Host.UseSerilog((context, services, configuration) => {
    configuration.ReadFrom.Configuration(context.Configuration);
    configuration.WriteTo.ApplicationInsights(
        services.GetRequiredService<TelemetryConfiguration>(), TelemetryConverter.Traces
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

// Bind to Azure's expected host/port
//Test fix for azure linux operating system
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Add($"http://0.0.0.0:{port}");

app.Run();
