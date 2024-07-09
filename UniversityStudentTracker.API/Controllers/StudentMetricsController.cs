using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityStudentTracker.API.Services;
using UniversityStudentTracker.API.Utils.Enums;

namespace UniversityStudentTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = nameof(UserRole.Student))]
public class StudentMetricsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly StudentMetricsService _studentMetricsService;

    public StudentMetricsController(StudentMetricsService studentMetricsService, IMapper mapper)
    {
        _studentMetricsService = studentMetricsService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetStatistics()
    {
        var currentDate = DateTime.UtcNow;

        var startDate = new DateTime(currentDate.Year, currentDate.Month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        var startYear = new DateTime(currentDate.Year, 1, 1);
        var endYear = new DateTime(currentDate.Year, 12, 31, 23, 59, 59);

        var studySessionsByMonth = await _studentMetricsService.GetStudySessionsByRangeAsync(startDate, endDate);
        var breaksByMonth = await _studentMetricsService.GetBreaksByRangeAsync(startDate, endDate);
        var studySessionsByYear = await _studentMetricsService.GetStudySessionsByRangeAsync(startYear, endYear);
        var breaksByYear = await _studentMetricsService.GetBreaksByRangeAsync(startYear, endYear);

        var metrics = await _studentMetricsService.GetStudentMetricsAsync(studySessionsByMonth, breaksByMonth,
            studySessionsByYear, breaksByYear);

        return Ok(metrics);
    }
}