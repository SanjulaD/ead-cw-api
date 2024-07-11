using Microsoft.EntityFrameworkCore;
using UniversityStudentTracker.API.Contexts;
using UniversityStudentTracker.API.Models.Domains;

namespace UniversityStudentTracker.API.Repositories;

public class StudentMetricsRepository : IStudentMetricsInterface
{
    private readonly StudentPerformance _studentPerformanceDbContext;
    private readonly IUserInterface _userInterface;

    public StudentMetricsRepository(StudentPerformance studentPerformance, IUserInterface userInterface)
    {
        _studentPerformanceDbContext = studentPerformance;
        _userInterface = userInterface;
    }

    public async Task<List<StudySession>> GetStudySessionsByRangeAsync(DateTime startDate, DateTime endDate)
    {
        var userId = _userInterface.GetUserId();

        return await _studentPerformanceDbContext.StudySessions
            .Where(ss => ss.UserID == userId && ss.Date >= startDate && ss.Date <= endDate)
            .ToListAsync();
    }

    public async Task<List<Break>> GetBreaksByRangeAsync(DateTime startDate, DateTime endDate)
    {
        var userId = _userInterface.GetUserId();

        return await _studentPerformanceDbContext.Breaks
            .Where(b => b.UserID == userId && b.Date >= startDate && b.Date <= endDate)
            .ToListAsync();
    }
}