using UniversityStudentTracker.API.Helpers;
using UniversityStudentTracker.API.Models.Domains;
using UniversityStudentTracker.API.Models.DTO.Admin;
using UniversityStudentTracker.API.Repositories;

namespace UniversityStudentTracker.API.Services;

public class AdminService : IAdminInterface
{
    private readonly IAdminInterface _adminInterface;

    public AdminService(IAdminInterface adminInterface)
    {
        _adminInterface = adminInterface;
    }

    public async Task<List<StudySession>> GetStudySessionsByRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _adminInterface.GetStudySessionsByRangeAsync(startDate, endDate);
    }

    public async Task<List<Break>> GetBreaksByRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _adminInterface.GetBreaksByRangeAsync(startDate, endDate);
    }

    // public async Task<int> GetTotalNumberOfStudentsAsync()
    // {
    //     return await _adminInterface.GetTotalNumberOfStudentsAsync();
    // }

    public async Task<int> GetTotalNumberOfStudySessionsAsync(DateTime startDate, DateTime endDate)
    {
        return await _adminInterface.GetTotalNumberOfStudySessionsAsync(startDate, endDate);
    }

    public async Task<int> GetTotalNumberOfBreaksAsync(DateTime startDate, DateTime endDate)
    {
        return await _adminInterface.GetTotalNumberOfBreaksAsync(startDate, endDate);
    }

    public async Task<int> GetTotalStudyTimeLoggedAsync(DateTime startDate, DateTime endDate)
    {
        return await _adminInterface.GetTotalStudyTimeLoggedAsync(startDate, endDate);
    }

    public async Task<int> GetTotalBreakTimeLoggedAsync(DateTime startDate, DateTime endDate)
    {
        return await _adminInterface.GetTotalBreakTimeLoggedAsync(startDate, endDate);
    }

    public Task<AdminMetricsDTO> GetStudentMetricsAsync(
        int totalNumberOfStudents,
        int totalNumberOfStudySessions,
        int totalNumberOfBreakTimeMinutes,
        int totalStudyTimeLogged,
        int totalBreakTimeLogged,
        List<StudySession> studySessionsByYear,
        List<Break> breaksByYear,
        List<StudySession> studySessionsByWeek
    )
    {
        var totalStudyTimeBySubjectByWeek = new Dictionary<string, int>();

        var (monthlyStudyTimeHours, monthlyBreakTimeHours, totalStudyTimeHoursByYear, totalBreakTimeHoursByYear) =
            StatisticsHelper.CalculateYearlyStatisticsByMonth(studySessionsByYear, breaksByYear);

        foreach (var session in studySessionsByWeek)
            if (totalStudyTimeBySubjectByWeek.ContainsKey(session.Subject))
                totalStudyTimeBySubjectByWeek[session.Subject] += session.DurationMinutes;
            else
                totalStudyTimeBySubjectByWeek[session.Subject] = session.DurationMinutes;

        return Task.FromResult(new AdminMetricsDTO
        {
            TotalNumberOfStudents = totalNumberOfStudents,
            TotalNumberOfStudySessions = totalNumberOfStudySessions,
            TotalNumberOfBreaks = totalNumberOfBreakTimeMinutes,
            TotalStudyTimeLogged = totalStudyTimeLogged,
            TotalBreakTimeLogged = totalBreakTimeLogged,
            MonthlyStudyTimeHours = monthlyStudyTimeHours,
            MonthlyBreakTimeHours = monthlyBreakTimeHours,
            TotalStudyTimeHoursByYear = totalStudyTimeHoursByYear,
            TotalBreakTimeHoursByYear = totalBreakTimeHoursByYear,
            TotalStudyTimeBySubjectByWeek = totalStudyTimeBySubjectByWeek
        });
    }
}