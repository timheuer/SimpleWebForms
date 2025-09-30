using SimpleBlazorApp.Extensions;
using SimpleBlazorApp.Middleware;
using SimpleBlazorApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add HttpContextAccessor for error handling
builder.Services.AddHttpContextAccessor();

// Register application services
builder.Services.AddScoped<IGreetingService, GreetingService>();

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
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
app.UseAntiforgery();

// Map Razor components with interactive server rendering
app.MapRazorComponents<SimpleBlazorApp.App>()
    .AddInteractiveServerRenderMode();

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
