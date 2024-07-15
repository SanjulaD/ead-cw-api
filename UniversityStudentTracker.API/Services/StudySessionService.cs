using UniversityStudentTracker.API.Models.Domains;
using UniversityStudentTracker.API.Repositories;

namespace UniversityStudentTracker.API.Services;

public class StudySessionService : IStudySessionInterface
{
    private readonly IStudySessionInterface _studySessionInterface;

    public StudySessionService(IStudySessionInterface studySessionInterface)
    {
        _studySessionInterface = studySessionInterface;
    }

    public async Task<List<StudySession>> GetAllAsync()
    {
        return await _studySessionInterface.GetAllAsync();
    }

    public async Task<StudySession> CreateAsync(StudySession studySession)
    {
        return await _studySessionInterface.CreateAsync(studySession);
    }

    public async Task<StudySession?> GetByIdAsync(Guid id)
    {
        return await _studySessionInterface.GetByIdAsync(id);
    }

    public async Task<StudySession?> DeleteAsync(Guid id)
    {
        return await _studySessionInterface.DeleteAsync(id);
    }

    public async Task<IEnumerable<StudySession>> GetStudySessionsBySubject(string subject)
    {
        return await _studySessionInterface.GetStudySessionsBySubject(subject);
    }
}