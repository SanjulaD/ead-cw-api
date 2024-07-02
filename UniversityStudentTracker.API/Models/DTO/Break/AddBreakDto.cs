using System.ComponentModel.DataAnnotations;

namespace UniversityStudentTracker.API.Models.DTO.Break;

public class AddBreakDto
{
    [Required(ErrorMessage = "Date is required.")]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Duration in minutes is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Duration must be at least 1 minute.")]
    public int DurationMinutes { get; set; }
}