namespace UniversityStudentTracker.API.Models.DTO.Admin;

public class AdminMetricsDTO
{
    public int TotalNumberOfStudents { get; set; }
    public int TotalNumberOfStudySessions { get; set; }
    public int TotalNumberOfBreaks { get; set; }
    public int TotalStudyTimeLogged { get; set; }
    public int TotalBreakTimeLogged { get; set; }
    public int[] MonthlyStudyTimeHours { get; set; }
    public int[] MonthlyBreakTimeHours { get; set; }
    public int TotalStudyTimeHoursByYear { get; set; }
    public int TotalBreakTimeHoursByYear { get; set; }

    public Dictionary<string, int> TotalStudyTimeBySubjectByWeek { get; set; }
}