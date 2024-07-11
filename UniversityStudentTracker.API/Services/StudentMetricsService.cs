using UniversityStudentTracker.API.Helpers;
using UniversityStudentTracker.API.Models.Domains;
using UniversityStudentTracker.API.Models.DTO.Student;
using UniversityStudentTracker.API.Repositories;

namespace UniversityStudentTracker.API.Services;

public class StudentMetricsService : IStudentMetricsInterface
{
    private readonly IStudentMetricsInterface _studentMetricsInterface;

    public StudentMetricsService(IStudentMetricsInterface studentMetricsInterface)
    {
        _studentMetricsInterface = studentMetricsInterface;
    }

    public async Task<List<StudySession>> GetStudySessionsByRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _studentMetricsInterface.GetStudySessionsByRangeAsync(startDate, endDate);
    }

    public async Task<List<Break>> GetBreaksByRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _studentMetricsInterface.GetBreaksByRangeAsync(startDate, endDate);
    }

    public Task<StudentMetricsDto> GetStudentMetricsAsync(
        List<StudySession> studySessionsByMonth,
        List<Break> breaksByMonth,
        List<StudySession> studySessionsByYear,
        List<Break> breaksByYear,
        List<StudySession> studySessionsByWeek)
    {
        var studyTimeBySubject = new Dictionary<string, int>();
        var monthlyMetrics = StatisticsHelper.CalculateMonthlyStatistics(studySessionsByMonth, breaksByMonth);

        var (monthlyStudyTimeHours, monthlyBreakTimeHours, totalStudyTimeHoursByYear, totalBreakTimeHoursByYear) =
            StatisticsHelper.CalculateYearlyStatisticsByMonth(studySessionsByYear, breaksByYear);

        foreach (var session in studySessionsByWeek)
            if (studyTimeBySubject.ContainsKey(session.Subject))
                studyTimeBySubject[session.Subject] += session.DurationMinutes;
            else
                studyTimeBySubject[session.Subject] = session.DurationMinutes;

        return Task.FromResult(new StudentMetricsDto
        {
            TotalStudyTimeMinutes = monthlyMetrics.totalStudyTimeMinutes,
            AverageStudySessionDuration = monthlyMetrics.averageStudySessionDuration,
            TotalBreakTimeMinutes = monthlyMetrics.totalBreakTimeMinutes,
            NumberOfStudySessions = monthlyMetrics.numberOfStudySessions,
            MonthlyStudyTimeHours = monthlyStudyTimeHours,
            MonthlyBreakTimeHours = monthlyBreakTimeHours,
            TotalStudyTimeHoursByYear = totalStudyTimeHoursByYear,
            TotalBreakTimeHoursByYear = totalBreakTimeHoursByYear,
            TotalStudyTimeBySubjectByWeek = studyTimeBySubject
        });
    }
}