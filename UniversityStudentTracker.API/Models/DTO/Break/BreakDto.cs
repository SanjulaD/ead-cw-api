namespace UniversityStudentTracker.API.Models.DTO.Break;

public class BreakDto
{
    public Guid BreakID { get; set; }
    public DateTime Date { get; set; }
    public int DurationMinutes { get; set; }
}