using Microsoft.AspNetCore.Mvc;
using CosmoAppAPI.Services;

namespace CosmoAppAPI.Controllers;

[ApiController]
[Route("apod")]
public class CosmoController : ControllerBase
{
	private readonly INasaService _nasaService;
    public CosmoController(INasaService nasaService) {
 		_nasaService = nasaService;
	}

	[HttpGet("today")]
	public async Task<IActionResult> getApodToday()
	{
		var apodData = await _nasaService.getTodayApod();

		if (apodData == null)
		{
			return StatusCode(500, "The data from NASA could not be retrieved.");
		}
		
		return Ok(apodData);
	}
    
	[HttpGet("photos/{date}")]
	public async Task<IActionResult> getApodPhotos(string date)
	{
		if (!DateOnly.TryParseExact(date, "yyyy-mm-dd", out var parsedDate))
		{
			return BadRequest("The date format is invalid. Use format yyyy-mm-dd.");
		}

		if (parsedDate > DateOnly.FromDateTime(DateTime.Today))
		{
			return BadRequest("The date can't be in the future!");
		}

		var photo = await _nasaService.getApodByDate(date);

		if (photo == null)
		{
			return NotFound($"There is no image for the date {date}.");
		}

		return Ok(photo);
	}
}