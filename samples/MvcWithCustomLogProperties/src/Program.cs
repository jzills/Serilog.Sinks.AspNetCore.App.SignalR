using Microsoft.AspNetCore.SignalR;
using MvcWithCustomLogProperties.Enrichers;
using MvcWithCustomLogProperties.Hubs;
using Serilog;
using Serilog.Sinks.AspNetCore.App.SignalR.Extensions;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR()
    .AddJsonProtocol(options => 
        options.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);

builder.Services.AddSingleton<UserEnricher>();

// Required to register SignalR Hub for Serilog use
builder.Services.AddSerilogHub<SampleHub>();
builder.Services.AddSerilog((serviceProvider, loggerConfiguration) => 
    loggerConfiguration
        .Enrich.With<UserEnricher>()
        .WriteTo.SignalR<SampleHub>(
            serviceProvider, 
            (context, message, logEvent) =>
            {
                // Only push events with our custom properties
                var properties = logEvent.Properties;
                if (properties.TryGetValue("Name",  out var nameProperty) &&
                    properties.TryGetValue("Email", out var emailProperty))
                {
                    string name;
                    using (var writer = new StringWriter())
                    {
                        nameProperty.Render(writer);
                        name = writer.ToString();
                    }

                    string email;
                    using (var writer = new StringWriter())
                    {
                        emailProperty.Render(writer);
                        email = writer.ToString();
                    }
                    
                    // Push the formatted message with our custom properties
                    return context.Clients.All.SendAsync(
                        "LogUser", 
                        message, 
                        name, 
                        email
                    );
                }
                else
                {
                    return Task.CompletedTask;
                }
            }
        ));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSerilogRequestLogging();

app.UseRouting();

app.UseAuthorization();

app.MapHub<SampleHub>("/sample");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
