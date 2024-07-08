using UniversityStudentTracker.API.Models.Domains;
using UniversityStudentTracker.API.Models.DTO.Student;
using UniversityStudentTracker.API.Repositories;

namespace UniversityStudentTracker.API.Services;

public class StudentMetricsService : IStudentMetricsInterface
{
    private readonly IStudentMetricsInterface _studentMetricsRepository;

    public StudentMetricsService(IStudentMetricsInterface studentMetricsRepository)
    {
        _studentMetricsRepository = studentMetricsRepository;
    }

    public async Task<List<StudySession>> GetStudySessionsByRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _studentMetricsRepository.GetStudySessionsByRangeAsync(startDate, endDate);
    }

    public async Task<List<Break>> GetBreaksByRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _studentMetricsRepository.GetBreaksByRangeAsync(startDate, endDate);
    }

    public async Task<StudentMetricsDto> GetStudentMetricsAsync(IEnumerable<StudySession> studySessions,
        IEnumerable<Break> breaks)
    {
        var studySessionList = studySessions.ToList();
        var breakList = breaks.ToList();

        var totalStudyTimeMinutes = studySessionList.Sum(ss => ss.DurationMinutes);
        Console.WriteLine("-------------------------111111--------------");
        var averageStudySessionDuration =
            studySessionList.Count != 0 ? studySessionList.Average(ss => ss.DurationMinutes) : 0;
        Console.WriteLine("----------------222222-----------------------");
        var totalBreakTimeMinutes = 10;
        Console.WriteLine("------------3333333---------------------------");
        var numberOfStudySessions = studySessionList.Count;

        Console.WriteLine("---------------------------------------");

        return new StudentMetricsDto
        {
            TotalStudyTimeMinutes = totalStudyTimeMinutes,
            AverageStudySessionDuration = averageStudySessionDuration,
            TotalBreakTimeMinutes = totalBreakTimeMinutes,
            NumberOfStudySessions = numberOfStudySessions
        };
    }
}