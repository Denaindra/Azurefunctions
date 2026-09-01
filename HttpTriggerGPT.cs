using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace homecode.Function;

public class HttpTriggerGPT
{
    private readonly ILogger<HttpTriggerGPT> _logger;
    private readonly HttpClient _httpClient;

    public HttpTriggerGPT(ILogger<HttpTriggerGPT> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    [Function("HttpTriggerGPT")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("HTTP trigger function processing request at {Timestamp}", DateTime.UtcNow);

        try
        {
            // Extract query parameter
            string? userId = req.Query["userId"];

            if (string.IsNullOrEmpty(userId))
            {
                userId = "1"; // Default user ID
            }

            _logger.LogInformation("Fetching data for userId: {UserId}", userId);

            // Call JSONPlaceholder public API to get user data
            var userResponse = await GetUserDataFromPublicApi(userId);

            if (userResponse == null)
            {
                return new NotFoundObjectResult(new {  error = "User not found" });
            }

            // Parse and enhance the response
            var enhancedResponse = new
            {
                timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                userId = userId,
                userData = userResponse,
                message = "Successfully retrieved user data from public API",
                source = "JSONPlaceholder API"
            };

            _logger.LogInformation("Successfully processed request for userId: {UserId}", userId);

            return new OkObjectResult(enhancedResponse);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error calling external API");
            return new StatusCodeResult(StatusCodes.Status503ServiceUnavailable);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    private async Task<dynamic?> GetUserDataFromPublicApi(string userId)
    {
        try
        {
            _logger.LogInformation("Calling JSONPlaceholder API for userId: {UserId}", userId);

            // Call JSONPlaceholder API (free public API for testing)
            var response = await _httpClient.GetAsync($"https://jsonplaceholder.typicode.com/users/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("API returned status code: {StatusCode}", response.StatusCode);
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Received response from API: {ContentLength} bytes", content.Length);

            // Deserialize JSON response
            var userData = JsonSerializer.Deserialize<dynamic>(content);
            return userData;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching user data from public API");
            throw;
        }
    }
}