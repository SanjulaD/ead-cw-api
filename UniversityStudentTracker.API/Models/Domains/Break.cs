namespace UniversityStudentTracker.API.Models.Domains;

public class Break
{
    public Guid BreakID { get; set; }
    public Guid UserID { get; set; }
    public DateTime Date { get; set; }
    public int DurationMinutes { get; set; }
}