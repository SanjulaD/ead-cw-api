using UniversityStudentTracker.API.Models.Domains;

namespace UniversityStudentTracker.API.Repositories;

public interface IStudySessionInterface
{
    Task<List<StudySession>> GetAllAsync();
    Task<StudySession> CreateAsync(StudySession studySession);
    Task<StudySession?> GetByIdAsync(Guid id);
    Task<StudySession?> DeleteAsync(Guid id);
}