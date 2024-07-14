namespace UniversityStudentTracker.API.Models.DTO.Prediction;

public class PredictionRequestDto
{
    public string subject { get; set; }
    public float average_study_time { get; set; }
    public float average_break_time { get; set; }
}