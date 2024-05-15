using Serilog;
using Serilog.Sinks.SignalR;
using Serilog.Sinks.SignalR.Extensions;
using Web.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

// Required to register SignalR Hub for Serilog use
builder.Services.AddSerilogHub<SampleHub>();
builder.Services.AddSerilog((serviceProvider, loggerConfiguration) => 
{
    loggerConfiguration.WriteTo
        .SignalR<SampleHub>(serviceProvider, "ReceiveMessage", null);
});

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
