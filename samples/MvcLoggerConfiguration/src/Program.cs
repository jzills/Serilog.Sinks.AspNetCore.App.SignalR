using Microsoft.AspNetCore.SignalR;
using Mvc.Hubs;
using Serilog;
using Serilog.Sinks.AspNetCore.App.SignalR.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

// Method 1
// Register the default SignalR Hub for Serilog. This method
// doesn't require any hubs to exist in the consuming codebase
// as a default one is registered internally.
builder.Services.AddDefaultSerilogHub();
builder.Services.AddSerilog((serviceProvider, loggerConfiguration) => 
    loggerConfiguration.WriteTo.SignalR(
        serviceProvider, 
        "ReceiveEvent"
    ));

// Method 2
// Register your own hub for Serilog and specify the hub method
// used to push events.
builder.Services.AddSerilogHub<SampleHub>();
builder.Services.AddSerilog((serviceProvider, loggerConfiguration) =>
    loggerConfiguration.WriteTo.SignalR<SampleHub>(
        serviceProvider, 
        "ReceiveEvent"
    ));

// Method 3
// Register your own hub for Serilog and define the delegate
// to push events. This allows the most flexibility as you have
// direct access to the context representing the specified hub. There is
// also an overload that accepts the Serilog event for further processing.
builder.Services.AddSerilogHub<SampleHub>();
builder.Services.AddSerilog((serviceProvider, loggerConfiguration) =>
    loggerConfiguration.WriteTo.SignalR<SampleHub>(
        serviceProvider, 
        (context, message) => context.Clients.All.SendAsync("ReceiveEvent", message)
        //(context, message, logEvent) => context.Clients.All.SendAsync("ReceiveEvent", message, logEvent)
    ));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSerilogRequestLogging();

app.UseRouting();

app.UseAuthorization();

// Method 1
app.MapDefaultSerilogHub("/sample");

// Method 2 & 3
app.MapHub<SampleHub>("/sample");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
