using System.Text.Json;
using CosmoAppAPI.Dtos;

namespace CosmoAppAPI.Services;

public interface INasaService
{
    Task<ApodResponseDto?> getTodayApod();
    Task<ApodPhotoDto?> getApodByDate(string date);
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

    public async Task<ApodPhotoDto?> getApodByDate(string date)
    {
        string url = $"https://api.nasa.gov/planetary/apod?api_key={_apiKey}&date={date}";
        
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

        var fullResponse = JsonSerializer.Deserialize<ApodResponseDto>(content, options);

        if (fullResponse == null)
        {
            Console.WriteLine("No apod found for date " + date);
            return null;
        }

        return new ApodPhotoDto
        {
            Date = fullResponse.Date,
            Title = fullResponse.Title,
            Url = fullResponse.Url
        };
    }
}