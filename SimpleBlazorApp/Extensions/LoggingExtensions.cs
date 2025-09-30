namespace SimpleBlazorApp.Extensions;

/// <summary>
/// Extension methods for enhanced logging functionality
/// </summary>
public static class LoggingExtensions
{
    /// <summary>
    /// Logs application startup information
    /// </summary>
    public static void LogApplicationStartup(this ILogger logger, string applicationName, string environment)
    {
        logger.LogInformation("Application {ApplicationName} starting up in {Environment} environment", 
            applicationName, environment);
    }

    /// <summary>
    /// Logs application shutdown information
    /// </summary>
    public static void LogApplicationShutdown(this ILogger logger, string applicationName)
    {
        logger.LogInformation("Application {ApplicationName} shutting down", applicationName);
    }

    /// <summary>
    /// Logs user interaction events
    /// </summary>
    public static void LogUserInteraction(this ILogger logger, string action, string component, object? additionalData = null)
    {
        using var scope = logger.BeginScope("UserInteraction");
        
        if (additionalData != null)
        {
            logger.LogInformation("User performed {Action} in {Component} with data: {@AdditionalData}", 
                action, component, additionalData);
        }
        else
        {
            logger.LogInformation("User performed {Action} in {Component}", action, component);
        }
    }

    /// <summary>
    /// Logs performance metrics
    /// </summary>
    public static void LogPerformanceMetric(this ILogger logger, string operation, TimeSpan duration, bool isSuccess = true)
    {
        var level = duration.TotalMilliseconds > 1000 ? LogLevel.Warning : LogLevel.Debug;
        
        logger.Log(level, "Performance: {Operation} completed in {Duration}ms with success: {IsSuccess}", 
            operation, duration.TotalMilliseconds, isSuccess);
    }

    /// <summary>
    /// Logs security-related events
    /// </summary>
    public static void LogSecurityEvent(this ILogger logger, string eventType, string details, bool isSuccessful = true)
    {
        var level = isSuccessful ? LogLevel.Information : LogLevel.Warning;
        
        logger.Log(level, "Security Event: {EventType} - {Details} (Success: {IsSuccessful})", 
            eventType, details, isSuccessful);
    }

    /// <summary>
    /// Logs component lifecycle events
    /// </summary>
    public static void LogComponentLifecycle(this ILogger logger, string componentName, string lifecycleEvent)
    {
        logger.LogTrace("Component {ComponentName} - {LifecycleEvent}", componentName, lifecycleEvent);
    }
}