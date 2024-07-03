using Microsoft.EntityFrameworkCore;
using UniversityStudentTracker.API.Contexts;
using UniversityStudentTracker.API.Models.Domains;

namespace UniversityStudentTracker.API.Repositories;

public class BreakRepository : IBreakInterface
{
    private readonly StudentPerformance _studentPerformanceDbContext;
    private readonly IUserInterface _userInterface;

    public BreakRepository(StudentPerformance studentPerformanceDbContext, IUserInterface userInterface)
    {
        _studentPerformanceDbContext = studentPerformanceDbContext;
        _userInterface = userInterface;
    }

    public async Task<List<Break>> GetAllAsync()
    {
        var userId = _userInterface.GetUserId();

        return await _studentPerformanceDbContext.Breaks
            .Where(b => b.UserID == userId)
            .ToListAsync();
    }

    public async Task<Break> CreateAsync(Break studyBreak)
    {
        studyBreak.UserID = _userInterface.GetUserId();
        await _studentPerformanceDbContext.Breaks.AddAsync(studyBreak);
        await _studentPerformanceDbContext.SaveChangesAsync();

        return studyBreak;
    }

    public async Task<Break?> GetByIdAsync(Guid id)
    {
        var userId = _userInterface.GetUserId();
        return await _studentPerformanceDbContext.Breaks
            .FirstOrDefaultAsync(x => x.BreakID == id && x.UserID == userId);
    }

    public async Task<Break?> DeleteAsync(Guid id)
    {
        var userId = _userInterface.GetUserId();

        var existingBreak = await _studentPerformanceDbContext.Breaks
            .FirstOrDefaultAsync(x => x.BreakID == id && x.UserID == userId);
        if (existingBreak == null) return null;

        _studentPerformanceDbContext.Breaks.Remove(existingBreak);
        await _studentPerformanceDbContext.SaveChangesAsync();

        return existingBreak;
    }
}