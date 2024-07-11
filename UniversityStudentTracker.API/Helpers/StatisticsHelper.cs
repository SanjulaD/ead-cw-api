using UniversityStudentTracker.API.Models.Domains;

namespace UniversityStudentTracker.API.Helpers;

public class StatisticsHelper
{
    public static (int[] monthlyStudyTimeHours, int[] monthlyBreakTimeHours, int totalStudyTimeHoursByYear, int
        totalBreakTimeHoursByYear)
        CalculateYearlyStatisticsByMonth(List<StudySession> studySessions, List<Break> breaks)
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

    public static (int totalStudyTimeMinutes, double averageStudySessionDuration, int totalBreakTimeMinutes, int
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
}