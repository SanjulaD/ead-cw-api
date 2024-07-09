namespace UniversityStudentTracker.API.Models.DTO.Student;

public class StudentMetricsDto
{
    public int TotalStudyTimeMinutes { get; set; }
    public double AverageStudySessionDuration { get; set; }
    public int TotalBreakTimeMinutes { get; set; }
    public int NumberOfStudySessions { get; set; }
    public int[] MonthlyStudyTimeHours { get; set; }
    public int[] MonthlyBreakTimeHours { get; set; }
    public int TotalStudyTimeHoursByYear { get; set; }
    public int TotalBreakTimeHoursByYear { get; set; }

    public Dictionary<string, int> TotalStudyTimeBySubjectByWeek { get; set; }
}