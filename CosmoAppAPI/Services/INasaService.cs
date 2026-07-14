namespace CosmoAppAPI.Services;

using CosmoAppAPI.Dtos;

public interface INasaService
{
    Task<ApodResponseDto?> getTodayApod();
}