using Microsoft.EntityFrameworkCore;
using UniversityStudentTracker.API.Contexts;
using UniversityStudentTracker.API.Models.Domains;
using UniversityStudentTracker.API.Repositories;

namespace UniversityStudentTracker.API.Services;

public class BreaksService : IBreaksRepository
{
    private readonly StudentPerformance _studentPerformanceDbContext;
    private readonly IUserAccessor _userAccessor;

    public BreaksService(StudentPerformance studentPerformanceDbContext, IUserAccessor userAccessor)
    {
        _studentPerformanceDbContext = studentPerformanceDbContext;
        _userAccessor = userAccessor;
    }

    public async Task<List<Break>> GetAllAsync()
    {
        var userId = _userAccessor.GetUserId();

        return await _studentPerformanceDbContext.Breaks
            .Where(b => b.UserID == userId)
            .ToListAsync();
    }

    public async Task<Break> CreateAsync(Break studyBreak)
    {
        studyBreak.UserID = _userAccessor.GetUserId();
        await _studentPerformanceDbContext.Breaks.AddAsync(studyBreak);
        await _studentPerformanceDbContext.SaveChangesAsync();

        return studyBreak;
    }

    public async Task<Break?> GetByIdAsync(Guid id)
    {
        var userId = _userAccessor.GetUserId();
        return await _studentPerformanceDbContext.Breaks
            .FirstOrDefaultAsync(x => x.BreakID == id && x.UserID == userId);
    }

    public async Task<Break?> DeleteAsync(Guid id)
    {
        var userId = _userAccessor.GetUserId();

        var existingBreak = await _studentPerformanceDbContext.Breaks
            .FirstOrDefaultAsync(x => x.BreakID == id && x.UserID == userId);
        if (existingBreak == null) return null;

        _studentPerformanceDbContext.Breaks.Remove(existingBreak);
        await _studentPerformanceDbContext.SaveChangesAsync();

        return existingBreak;
    }
}