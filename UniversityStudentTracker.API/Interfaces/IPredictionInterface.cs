using UniversityStudentTracker.API.Models.Domains;

namespace UniversityStudentTracker.API.Repositories;

public interface IPredictionInterface
{
    Task<Prediction> CreatePredictionAsync(Prediction prediction);
    Task<List<StudySession>> GetStudySessionsByUserAndSubjectAsync(string subject);
    Task<List<Break>> GetBreaksByUserAsync();
}