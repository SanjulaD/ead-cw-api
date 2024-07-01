using Microsoft.EntityFrameworkCore;
using UniversityStudentTracker.API.Contexts;
using UniversityStudentTracker.API.Models.Domains;
using UniversityStudentTracker.API.Repositories;

namespace UniversityStudentTracker.API.Services;

public class BreaksService : IBreaksRepository
{
    private readonly StudentPerformance _studentPerformanceDbContext;

    public BreaksService(StudentPerformance studentPerformanceDbContext)
    {
        _studentPerformanceDbContext = studentPerformanceDbContext;
    }

    public async Task<List<Break>> GetAllAsync()
    {
        return await _studentPerformanceDbContext.Breaks.ToListAsync();
    }

    public async Task<Break> CreateAsync(Break studyBreak)
    {
        await _studentPerformanceDbContext.Breaks.AddAsync(studyBreak);
        await _studentPerformanceDbContext.SaveChangesAsync();

        return studyBreak;
    }

    public async Task<Break?> GetByIdAsync(Guid id)
    {
        return await _studentPerformanceDbContext.Breaks.FirstOrDefaultAsync(x => x.BreakID == id);
    }

    public async Task<Break?> DeleteAsync(Guid id)
    {
        var existingBreak = await _studentPerformanceDbContext.Breaks.FindAsync(id);
        if (existingBreak == null) return null;

        _studentPerformanceDbContext.Breaks.Remove(existingBreak);
        await _studentPerformanceDbContext.SaveChangesAsync();

        return existingBreak;
    }
}