using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Mvc.Hubs;
using Serilog;
using Serilog.Sinks.AspNetCore.App.SignalR.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR()
    .AddJsonProtocol(options => 
        options.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);

// Required to register SignalR Hub for Serilog use
builder.Services.AddSerilogHub<SampleHub>();
builder.Services.AddSerilog((serviceProvider, loggerConfiguration) => 
    loggerConfiguration.WriteTo.SignalR<SampleHub>(
        serviceProvider, 
        (context, message, logEvent) => context.Clients.All.SendAsync("ReceiveEvent", message, logEvent)
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
