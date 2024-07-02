using System.ComponentModel.DataAnnotations;

namespace UniversityStudentTracker.API.Models.DTO.StudySession;

public class AddStudySessionDto
{
    [Required(ErrorMessage = "Subject is required.")]
    [StringLength(100, ErrorMessage = "Subject can't be longer than 100 characters.")]
    public string Subject { get; set; }

    [Required(ErrorMessage = "Date is required.")]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Duration in minutes is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Duration must be at least 1 minute.")]
    public int DurationMinutes { get; set; }
}