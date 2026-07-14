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
}