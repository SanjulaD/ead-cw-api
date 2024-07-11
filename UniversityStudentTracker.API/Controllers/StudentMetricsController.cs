using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityStudentTracker.API.Helpers;
using UniversityStudentTracker.API.Services;
using UniversityStudentTracker.API.Utils.Enums;

namespace UniversityStudentTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = nameof(UserRole.Student))]
public class StudentMetricsController : ControllerBase
{
    private readonly ILogger<BreaksController> _logger;
    private readonly StudentMetricsService _studentMetricsService;

    public StudentMetricsController(StudentMetricsService studentMetricsService, ILogger<BreaksController> logger)
    {
        _studentMetricsService = studentMetricsService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetStatistics()
    {
        var currentDate = DateTime.UtcNow;

        // Get start and end of week
        var (startOfWeek, endOfWeek) = DateHelper.GetStartEndOfDay(currentDate.AddDays(-(int)currentDate.DayOfWeek));

        // Get start and end of month
        var (startMonth, endMonth) = DateHelper.GetStartEndOfDay(new DateTime(currentDate.Year, currentDate.Month, 1));

        // Get start and end of year
        var (startYear, endYear) = DateHelper.GetStartEndOfDay(new DateTime(currentDate.Year, 1, 1));

        var studySessionsByMonth = await _studentMetricsService.GetStudySessionsByRangeAsync(startMonth, endMonth);
        var breaksByMonth = await _studentMetricsService.GetBreaksByRangeAsync(startMonth, endMonth);
        var studySessionsByYear = await _studentMetricsService.GetStudySessionsByRangeAsync(startYear, endYear);
        var breaksByYear = await _studentMetricsService.GetBreaksByRangeAsync(startYear, endYear);

        var studySessionsByWeek = await _studentMetricsService.GetStudySessionsByRangeAsync(startOfWeek, endOfWeek);

        var metrics = await _studentMetricsService.GetStudentMetricsAsync(studySessionsByMonth, breaksByMonth,
            studySessionsByYear, breaksByYear, studySessionsByWeek);

        _logger.LogInformation("Student metrics created successfully");

        return Ok(metrics);
    }
}