using UniversityStudentTracker.API.Models.Domains;

namespace UniversityStudentTracker.API.Repositories;

public interface IPredictionInterface
{
    Task AddPredictionAsync(Prediction prediction);
}