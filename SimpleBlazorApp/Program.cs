using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SimpleBlazorApp.Services;
using SimpleBlazorApp.Middleware;
using SimpleBlazorApp.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register server-side Blazor services so interactive events (SignalR) are enabled
builder.Services.AddServerSideBlazor();

// Add HttpContextAccessor for error handling
builder.Services.AddHttpContextAccessor();

// Register application services
builder.Services.AddScoped<IGreetingService, GreetingService>();

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole(options =>
{
    options.IncludeScopes = true;
    options.TimestampFormat = "yyyy-MM-dd HH:mm:ss.fff ";
});
builder.Logging.AddDebug();

// Add additional logging providers based on environment
if (builder.Environment.IsDevelopment())
{
    builder.Logging.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Debug);
    builder.Logging.AddFilter("Microsoft.AspNetCore.Http.Connections", LogLevel.Debug);
}
else
{
    // In production, add EventLog provider for Windows
    if (OperatingSystem.IsWindows())
    {
        builder.Logging.AddEventLog();
    }
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    // Use custom global exception handling in development for better error details
    app.UseGlobalExceptionHandling();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

// Ensure routing is enabled for hub endpoints and fallback page
app.UseRouting();

app.UseAntiforgery();

// Map Razor Pages endpoints (required for MapFallbackToPage and _Host page)
app.MapRazorPages();

// Map the Blazor hub to enable interactive server-side components (SignalR)
app.MapBlazorHub();

// Keep existing Razor components mapping (if using the newer Razor Components APIs)
app.MapRazorComponents<SimpleBlazorApp.App>()
    .AddInteractiveServerRenderMode();

// Map a fallback to the _Host page so the app can prerender and then activate on the client
app.MapFallbackToPage("/_Host");

// Log application startup
var logger = app.Services.GetRequiredService<ILogger<Program>>();
var appName = app.Configuration["ApplicationSettings:ApplicationName"] ?? "SimpleBlazorApp";
logger.LogApplicationStartup(appName, app.Environment.EnvironmentName);

// Configure application lifetime events
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
lifetime.ApplicationStopping.Register(() =>
{
    logger.LogApplicationShutdown(appName);
});

app.Run();
