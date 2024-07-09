using UniversityStudentTracker.API.Helpers;
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

    public Task<Dictionary<string, int>> GetStudyTimeBySubjectByRangeAsync(DateTime startDate, DateTime endDate)
    {
        throw new NotImplementedException();
    }

    public async Task<StudentMetricsDto> GetStudentMetricsAsync(
        List<StudySession> studySessionsByMonth,
        List<Break> breaksByMonth,
        List<StudySession> studySessionsByYear,
        List<Break> breaksByYear,
        List<StudySession> studySessionsByWeek)
    {
        var studyTimeBySubject = new Dictionary<string, int>();
        var monthlyMetrics = CalculateMonthlyStatistics(studySessionsByMonth, breaksByMonth);

        var (monthlyStudyTimeHours, monthlyBreakTimeHours, totalStudyTimeHoursByYear, totalBreakTimeHoursByYear) =
            CalculateYearlyStatisticsByMonth(studySessionsByYear, breaksByYear);

        foreach (var session in studySessionsByWeek)
            if (studyTimeBySubject.ContainsKey(session.Subject))
                studyTimeBySubject[session.Subject] += session.DurationMinutes;
            else
                studyTimeBySubject[session.Subject] = session.DurationMinutes;

        foreach (var group in studyTimeBySubject)
            Console.WriteLine("Key: {0} Value: {1}", group.Key, group.Value);

        return new StudentMetricsDto
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
        };
    }

    private (int totalStudyTimeMinutes, double averageStudySessionDuration, int totalBreakTimeMinutes, int
        numberOfStudySessions) CalculateMonthlyStatistics(
            List<StudySession> studySessions,
            List<Break> breaks)
    {
        var studySessionList = studySessions.ToList();
        var breakList = breaks.ToList();

        var totalStudyTimeMinutes = studySessionList.Sum(ss => ss.DurationMinutes);
        var averageStudySessionDuration =
            studySessionList.Count != 0 ? studySessionList.Average(ss => ss.DurationMinutes) : 0;
        var totalBreakTimeMinutes = breakList.Sum(b => b.DurationMinutes);
        var numberOfStudySessions = studySessionList.Count;

        return (totalStudyTimeMinutes, averageStudySessionDuration, totalBreakTimeMinutes, numberOfStudySessions);
    }

    private (int[] monthlyStudyTimeHours, int[] monthlyBreakTimeHours, int totalStudyTimeHoursByYear, int
        totalBreakTimeHoursByYear) CalculateYearlyStatisticsByMonth(
            List<StudySession> studySessions,
            List<Break> breaks)
    {
        var monthlyStudyTimeMinutes = new int[12];
        var monthlyBreakTimeMinutes = new int[12];

        foreach (var session in studySessions)
            monthlyStudyTimeMinutes[session.Date.Month - 1] += session.DurationMinutes;

        foreach (var breakItem in breaks)
            monthlyBreakTimeMinutes[breakItem.Date.Month - 1] += breakItem.DurationMinutes;

        var monthlyStudyTimeHours = TimeHelper.ConvertMinutesToHours(monthlyStudyTimeMinutes);
        var monthlyBreakTimeHours = TimeHelper.ConvertMinutesToHours(monthlyBreakTimeMinutes);

        var totalStudyTimeMinutesByYear = studySessions.Sum(ss => ss.DurationMinutes);
        var totalBreakTimeMinutesByYear = breaks.Sum(b => b.DurationMinutes);

        var totalStudyTimeHoursByYear = totalStudyTimeMinutesByYear / 60;
        var totalBreakTimeHoursByYear = totalBreakTimeMinutesByYear / 60;

        return (monthlyStudyTimeHours, monthlyBreakTimeHours, totalStudyTimeHoursByYear, totalBreakTimeHoursByYear);
    }
}