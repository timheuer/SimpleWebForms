namespace SimpleBlazorApp.Services
{
    /// <summary>
    /// Service interface for handling greeting functionality
    /// </summary>
    public interface IGreetingService
    {
        /// <summary>
        /// Generates a greeting message for the provided name
        /// </summary>
        /// <param name="name">The name to generate a greeting for</param>
        /// <returns>A greeting message</returns>
        string GenerateGreeting(string? name);

        /// <summary>
        /// Validates if the provided name is valid for greeting
        /// </summary>
        /// <param name="name">The name to validate</param>
        /// <returns>True if the name is valid, false otherwise</returns>
        bool ValidateName(string? name);
    }
}