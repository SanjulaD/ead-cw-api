using Microsoft.EntityFrameworkCore;
using UniversityStudentTracker.API.Contexts;
using UniversityStudentTracker.API.Models.Domains;

namespace UniversityStudentTracker.API.Repositories;

public class AdminRepository : IAdminInterface
{
    private readonly StudentPerformance _studentPerformanceDbContext;

    public AdminRepository(StudentPerformance studentPerformance)
    {
        _studentPerformanceDbContext = studentPerformance;
    }

    public async Task<List<StudySession>> GetStudySessionsByRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _studentPerformanceDbContext.StudySessions
            .Where(ss => ss.Date >= startDate && ss.Date <= endDate)
            .ToListAsync();
    }

    public async Task<List<Break>> GetBreaksByRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _studentPerformanceDbContext.Breaks
            .Where(b => b.Date >= startDate && b.Date <= endDate)
            .ToListAsync();
    }

    // public async Task<int> GetTotalNumberOfStudentsAsync()
    // {
    //     var users = await _userManager.GetUsersInRoleAsync(nameof(UserRole.Student));
    //     return users.Count;
    // }

    public async Task<int> GetTotalNumberOfStudySessionsAsync(DateTime startDate, DateTime endDate)
    {
        return await _studentPerformanceDbContext.StudySessions
            .Where(ss => ss.Date >= startDate && ss.Date <= endDate)
            .CountAsync();
    }

    public async Task<int> GetTotalNumberOfBreaksAsync(DateTime startDate, DateTime endDate)
    {
        Console.WriteLine(startDate);
        Console.WriteLine(endDate);

        return await _studentPerformanceDbContext.Breaks
            .Where(b => b.Date >= startDate && b.Date <= endDate)
            .CountAsync();
    }

    public async Task<int> GetTotalStudyTimeLoggedAsync(DateTime startDate, DateTime endDate)
    {
        return await _studentPerformanceDbContext.StudySessions
            .Where(ss => ss.Date >= startDate && ss.Date <= endDate)
            .SumAsync(ss => ss.DurationMinutes);
    }

    public async Task<int> GetTotalBreakTimeLoggedAsync(DateTime startDate, DateTime endDate)
    {
        return await _studentPerformanceDbContext.Breaks
            .Where(b => b.Date >= startDate && b.Date <= endDate)
            .SumAsync(b => b.DurationMinutes);
    }
}