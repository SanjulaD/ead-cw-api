namespace UniversityStudentTracker.API.Models.Domains;

public class StudySession
{
    public Guid StudySessionID { get; set; }
    public Guid UserID { get; set; }
    public string Subject { get; set; }
    public DateTime Date { get; set; }
    public int DurationMinutes { get; set; }
}