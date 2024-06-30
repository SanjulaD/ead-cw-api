namespace UniversityStudentTracker.API.Models.Domains;

public class Prediction
{
    public Guid PredictionID { get; set; }
    public Guid UserID { get; set; }
    public string Subject { get; set; }
    public double PredictedGrade { get; set; }
    public int PredictedKnowledgeLevel { get; set; }
    public DateTime PredictionDate { get; set; }
}