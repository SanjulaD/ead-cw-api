using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UniversityStudentTracker.API.Contexts;
using UniversityStudentTracker.API.Models.Domains;
using UniversityStudentTracker.API.Repositories;

namespace UniversityStudentTracker.API.Services;

public class StudySessionService : IStudySessionRepository
{
    private readonly StudentPerformance _studentPerformanceDbContext;
    private readonly UserManager<IdentityUser> _userManager;

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
}