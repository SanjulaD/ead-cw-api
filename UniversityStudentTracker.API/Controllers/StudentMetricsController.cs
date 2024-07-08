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

        Console.WriteLine(currentDate);
        Console.WriteLine(startDate);
        Console.WriteLine(endDate);

        var studySessionDomainModel = await _studentMetricsService.GetStudySessionsByRangeAsync(startDate, endDate);
        var breaksDomainModel = await _studentMetricsService.GetBreaksByRangeAsync(startDate, endDate);

        var metrics =
            await _studentMetricsService.GetStudentMetricsAsync(studySessionDomainModel, breaksDomainModel);


        Console.WriteLine(metrics);
        return Ok(metrics);
    }
}