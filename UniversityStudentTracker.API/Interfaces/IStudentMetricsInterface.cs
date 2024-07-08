using UniversityStudentTracker.API.Models.Domains;

namespace UniversityStudentTracker.API.Repositories;

public interface IStudentMetricsInterface
{
    Task<List<StudySession>> GetStudySessionsByRangeAsync(DateTime startDate, DateTime endDate);
    Task<List<Break>> GetBreaksByRangeAsync(DateTime startDate, DateTime endDate);
}