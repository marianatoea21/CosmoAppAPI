using System.Text.Json;
using CosmoAppAPI.Dtos;

namespace CosmoAppAPI.Services;

public interface INasaService
{
    Task<ApodResponseDto?> getTodayApod();
}

public class NasaService : INasaService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public NasaService(HttpClient httpClient, IConfiguration configuration)
    { 
        _httpClient = httpClient;
        _apiKey = configuration["NasaSettings:ApiKey"] ?? "DEMO_KEY";
    }

    public async Task<ApodResponseDto?> getTodayApod()
    {
        string url = $"https://api.nasa.gov/planetary/apod?api_key={_apiKey}";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
			var statusCode = response.StatusCode;
			var errorBody = await response.Content.ReadAsStringAsync();

			Console.WriteLine($"Request failed with status code: {statusCode} ({(int)statusCode})");
            Console.WriteLine($"Response body: {errorBody}");
			
            return null;
        }

        var content = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        
        return JsonSerializer.Deserialize<ApodResponseDto>(content, options);
    }
}