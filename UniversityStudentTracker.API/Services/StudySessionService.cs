using Microsoft.EntityFrameworkCore;
using UniversityStudentTracker.API.Contexts;
using UniversityStudentTracker.API.Models.Domains;
using UniversityStudentTracker.API.Repositories;

namespace UniversityStudentTracker.API.Services;

public class StudySessionService : IStudySessionRepository
{
    private readonly StudentPerformance _studentPerformanceDbContext;
    private readonly IUserAccessor _userAccessor;

    public StudySessionService(StudentPerformance studentPerformance, IUserAccessor userAccessor)
    {
        _studentPerformanceDbContext = studentPerformance;
        _userAccessor = userAccessor;
    }

    public async Task<List<StudySession>> GetAllAsync()
    {
        var userId = _userAccessor.GetUserId();

        return await _studentPerformanceDbContext.StudySessions
            .Where(s => s.UserID == userId)
            .ToListAsync();
    }

    public async Task<StudySession> CreateAsync(StudySession studySession)
    {
        studySession.UserID = _userAccessor.GetUserId();
        await _studentPerformanceDbContext.StudySessions.AddAsync(studySession);
        await _studentPerformanceDbContext.SaveChangesAsync();

        return studySession;
    }

    public async Task<StudySession?> GetByIdAsync(Guid id)
    {
        var userId = _userAccessor.GetUserId();
        return await _studentPerformanceDbContext.StudySessions
            .FirstOrDefaultAsync(x => x.StudySessionID == id && x.UserID == userId);
    }

    public async Task<StudySession?> DeleteAsync(Guid id)
    {
        var userId = _userAccessor.GetUserId();

        var existingStudySession = await _studentPerformanceDbContext.StudySessions
            .FirstOrDefaultAsync(x => x.StudySessionID == id && x.UserID == userId);

        if (existingStudySession == null) return null;

        _studentPerformanceDbContext.StudySessions.Remove(existingStudySession);
        await _studentPerformanceDbContext.SaveChangesAsync();

        return existingStudySession;
    }
}