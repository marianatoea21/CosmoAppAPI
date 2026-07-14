using System.Text.Json;
using CosmoAppAPI.Dtos;

namespace CosmoAppAPI.Services;

public class NasaService : INasaService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public NasaService(HttpClient httpClient, IConfiguration configuration)
    { 
        _httpClient = httpClient;
        _apiKey = configuration["NassaSettings:ApiKey"];
    }

    public async Task<ApodResponseDto?> getTodayApod()
    {
        string url = $"https://api.nasa.gov/planetary/apod?api_key={_apiKey}";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var content = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        
        return JsonSerializer.Deserialize<ApodResponseDto>(content, options);
    }
}