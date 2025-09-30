using System.Web;

namespace SimpleBlazorApp.Services
{
    /// <summary>
    /// Service implementation for handling greeting functionality
    /// </summary>
    public class GreetingService : IGreetingService
    {
        private readonly ILogger<GreetingService> _logger;

        public GreetingService(ILogger<GreetingService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Generates a greeting message for the provided name
        /// </summary>
        /// <param name="name">The name to generate a greeting for</param>
        /// <returns>A greeting message</returns>
        public string GenerateGreeting(string? name)
        {
            using var scope = _logger.BeginScope("GenerateGreeting for name: {Name}", name?.Length > 0 ? "[REDACTED]" : "[EMPTY]");

            _logger.LogDebug("Starting greeting generation process");

            try
            {
                // Validate the name first
                if (!ValidateName(name))
                {
                    _logger.LogInformation("Greeting generation failed due to invalid name input");
                    return "Please enter a name.";
                }

                // HTML encode the name to prevent XSS attacks
                var encodedName = HttpUtility.HtmlEncode((name ?? string.Empty).Trim());
                var greeting = $"hello {encodedName}";

                _logger.LogInformation("Successfully generated greeting for user input");
                _logger.LogDebug("Greeting generation completed successfully");

                return greeting;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred during greeting generation");
                return "An error occurred while generating your greeting. Please try again.";
            }
        }

        /// <summary>
        /// Validates if the provided name is valid for greeting
        /// </summary>
        /// <param name="name">The name to validate</param>
        /// <returns>True if the name is valid, false otherwise</returns>
        public bool ValidateName(string? name)
        {
            _logger.LogTrace("Validating name input");

            var isValid = !string.IsNullOrWhiteSpace(name);

            _logger.LogDebug("Name validation result: {IsValid}", isValid);

            return isValid;
        }
    }
}