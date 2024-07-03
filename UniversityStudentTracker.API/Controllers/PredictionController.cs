using Microsoft.AspNetCore.Mvc;
using UniversityStudentTracker.API.Models.DTO.Prediction;
using UniversityStudentTracker.API.Services;

namespace UniversityStudentTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PredictionController : ControllerBase
{
    private readonly PredictionService _predictionService;

    public PredictionController(PredictionService predictionService)
    {
        _predictionService = predictionService;
    }

    [HttpPost]
    [Route("Predict")]
    public async Task<IActionResult> Predict([FromBody] PredictionRequestDto predictionRequest)
    {
        if (predictionRequest == null) return BadRequest("Invalid input.");

        var result = await _predictionService.PredictAsync(predictionRequest.Subject);
        return Ok(result);
    }
}