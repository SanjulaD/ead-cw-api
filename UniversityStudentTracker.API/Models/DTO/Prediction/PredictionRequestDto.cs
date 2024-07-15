namespace UniversityStudentTracker.API.Models.DTO.Prediction;

public class PredictionRequestDto
{
    public string subject { get; set; }
    public double average_study_time { get; set; }
    public double average_break_time { get; set; }
}