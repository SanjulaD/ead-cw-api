namespace UniversityStudentTracker.API.Models.DTO.Student;

public class StudentMetricsDto
{
    public int TotalStudyTimeMinutes { get; set; }
    public double AverageStudySessionDuration { get; set; }
    public int TotalBreakTimeMinutes { get; set; }
    public int NumberOfStudySessions { get; set; }
}