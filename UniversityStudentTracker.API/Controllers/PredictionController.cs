using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityStudentTracker.API.Models.DTO.Prediction;

namespace UniversityStudentTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PredictionController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;

    public PredictionController(IMapper mapper, IHttpClientFactory httpClientFactory)
    {
        _mapper = mapper;
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("http://localhost:8000");
    }

    [HttpPost]
    [Route("Predict")]
    public async Task<PredictionResponseDto> Predict([FromBody] PredictionDto predictionDto)
    {
        try
        {
            var predictionRequest = _mapper.Map<PredictionRequestDto>(predictionDto);

            var response = await _httpClient.PostAsJsonAsync("/predict", predictionRequest);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var prediction = JsonSerializer.Deserialize<PredictionResponseDto>(jsonString);

            return prediction;
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Error calling Python service: {ex.Message}");
        }
    }
}