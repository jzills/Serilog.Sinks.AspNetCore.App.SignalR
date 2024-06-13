using Serilog;
using Serilog.Sinks.AspNetCore.App.SignalR.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

// Register the default SignalR Hub for Serilog.
builder.Services.AddDefaultSerilogHub();
builder.Services.AddSerilog((serviceProvider, loggerConfiguration) => 
    loggerConfiguration.WriteTo.SignalR(
        serviceProvider, 
        "ReceiveEvent"
    ));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSerilogRequestLogging();

app.UseRouting();

app.UseAuthorization();

app.MapDefaultSerilogHub("/sample");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
