using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityStudentTracker.API.Models.Domains;
using UniversityStudentTracker.API.Models.DTO.Prediction;
using UniversityStudentTracker.API.Services;

namespace UniversityStudentTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PredictionController : ControllerBase
{
    private readonly BreakService _breakService;
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;
    private readonly PredictionService _predictionService;
    private readonly StudySessionService _studySessionService;

    public PredictionController(IMapper mapper, IHttpClientFactory httpClientFactory,
        StudySessionService studySessionService, BreakService breakService, PredictionService predictionService)
    {
        _mapper = mapper;
        _studySessionService = studySessionService;
        _breakService = breakService;
        _predictionService = predictionService;
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("http://localhost:8000");
    }

    [HttpPost]
    [Route("Predict")]
    public async Task<PredictionResponseDto> Predict([FromBody] PredictionDto predictionDto)
    {
        try
        {
            var studySessions = await _studySessionService.GetStudySessionsBySubject(predictionDto.Subject);
            var breaks = await _breakService.GetAllAsync();

            var averageStudyTime = studySessions.Any() ? studySessions.Average(s => s.DurationMinutes) : 0;
            var averageBreakTime = breaks.Any() ? breaks.Average(b => b.DurationMinutes) : 0;

            var predictionRequest = new PredictionRequestDto
            {
                subject = predictionDto.Subject,
                average_study_time = averageStudyTime,
                average_break_time = averageBreakTime
            };

            var response = await _httpClient.PostAsJsonAsync("/predict", predictionRequest);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var predictionResponse = JsonSerializer.Deserialize<PredictionResponseDto>(jsonString);

            var prediction = new Prediction()
            {
                PredictionID = Guid.NewGuid(),
                Subject = predictionDto.Subject,
                PredictedGrade = Convert.ToDecimal(predictionResponse.PredictedGrade),
                PredictedKnowledgeLevel = Convert.ToInt32(predictionResponse.PredictedKnowledgeLevel),
                PredictionDate = DateTime.UtcNow
            };

            await _predictionService.AddPredictionAsync(prediction);
            return predictionResponse;
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Error calling Python service: {ex.Message}");
        }
    }
}