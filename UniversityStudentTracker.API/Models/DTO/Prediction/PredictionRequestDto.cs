using System.ComponentModel.DataAnnotations;

namespace UniversityStudentTracker.API.Models.DTO.Prediction;

public class PredictionRequestDto
{
    [Required(ErrorMessage = "Subject is required.")]
    [StringLength(100, ErrorMessage = "Subject can't be longer than 100 characters.")]
    public string Subject { get; set; }
}