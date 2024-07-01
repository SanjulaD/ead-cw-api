using Microsoft.EntityFrameworkCore;
using UniversityStudentTracker.API.Contexts;
using UniversityStudentTracker.API.Models.Domains;
using UniversityStudentTracker.API.Repositories;

namespace UniversityStudentTracker.API.Services;

public class StudySessionService : IStudySessionRepository
{
    private readonly StudentPerformance _studentPerformanceDbContext;

    public StudySessionService(StudentPerformance studentPerformance)
    {
        _studentPerformanceDbContext = studentPerformance;
    }

    public async Task<List<StudySession>> GetAllAsync()
    {
        return await _studentPerformanceDbContext.StudySessions.ToListAsync();
    }

    public async Task<StudySession> CreateAsync(StudySession studySession)
    {
        await _studentPerformanceDbContext.StudySessions.AddAsync(studySession);
        await _studentPerformanceDbContext.SaveChangesAsync();

        return studySession;
    }

    public async Task<StudySession?> GetByIdAsync(Guid id)
    {
        return await _studentPerformanceDbContext.StudySessions.FirstOrDefaultAsync(x => x.StudySessionID == id);
    }

    public async Task<StudySession?> DeleteAsync(Guid id)
    {
        var existingStudySession = await _studentPerformanceDbContext.StudySessions.FindAsync(id);
        if (existingStudySession == null) return null;

        _studentPerformanceDbContext.StudySessions.Remove(existingStudySession);
        await _studentPerformanceDbContext.SaveChangesAsync();

        return existingStudySession;
    }
}