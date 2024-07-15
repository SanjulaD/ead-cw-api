using Microsoft.EntityFrameworkCore;
using UniversityStudentTracker.API.Contexts;
using UniversityStudentTracker.API.Models.Domains;

namespace UniversityStudentTracker.API.Repositories;

public class StudySessionRepository : IStudySessionInterface
{
    private readonly StudentPerformance _studentPerformanceDbContext;
    private readonly IUserInterface _userInterface;

    public StudySessionRepository(StudentPerformance studentPerformance, IUserInterface userInterface)
    {
        _studentPerformanceDbContext = studentPerformance;
        _userInterface = userInterface;
    }

    public async Task<List<StudySession>> GetAllAsync()
    {
        var userId = _userInterface.GetUserId();

        return await _studentPerformanceDbContext.StudySessions
            .Where(s => s.UserID == userId)
            .ToListAsync();
    }

    public async Task<StudySession> CreateAsync(StudySession studySession)
    {
        studySession.UserID = _userInterface.GetUserId();
        await _studentPerformanceDbContext.StudySessions.AddAsync(studySession);
        await _studentPerformanceDbContext.SaveChangesAsync();

        return studySession;
    }

    public async Task<StudySession?> GetByIdAsync(Guid id)
    {
        var userId = _userInterface.GetUserId();
        return await _studentPerformanceDbContext.StudySessions
            .FirstOrDefaultAsync(x => x.StudySessionID == id && x.UserID == userId);
    }

    public async Task<StudySession?> DeleteAsync(Guid id)
    {
        var userId = _userInterface.GetUserId();

        var existingStudySession = await _studentPerformanceDbContext.StudySessions
            .FirstOrDefaultAsync(x => x.StudySessionID == id && x.UserID == userId);

        if (existingStudySession == null) return null;

        _studentPerformanceDbContext.StudySessions.Remove(existingStudySession);
        await _studentPerformanceDbContext.SaveChangesAsync();

        return existingStudySession;
    }

    public async Task<IEnumerable<StudySession>> GetStudySessionsBySubject(string subject)
    {
        return await _studentPerformanceDbContext.StudySessions.Where(ss => ss.Subject == subject).ToListAsync();
    }
}