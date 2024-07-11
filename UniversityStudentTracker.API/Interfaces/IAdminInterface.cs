using UniversityStudentTracker.API.Models.Domains;

namespace UniversityStudentTracker.API.Repositories;

public interface IAdminInterface
{
    Task<List<StudySession>> GetStudySessionsByRangeAsync(DateTime startDate, DateTime endDate);

    Task<List<Break>> GetBreaksByRangeAsync(DateTime startDate, DateTime endDate);

    // Task<int> GetTotalNumberOfStudentsAsync();
    Task<int> GetTotalNumberOfStudySessionsAsync(DateTime startDate, DateTime endDate);
    Task<int> GetTotalNumberOfBreaksAsync(DateTime startDate, DateTime endDate);
    Task<int> GetTotalStudyTimeLoggedAsync(DateTime startDate, DateTime endDate);
    Task<int> GetTotalBreakTimeLoggedAsync(DateTime startDate, DateTime endDate);
}