using UniversityStudentTracker.API.Models.Domains;
using UniversityStudentTracker.API.Repositories;

namespace UniversityStudentTracker.API.Services;

public class PredictionService : IPredictionInterface
{
    private readonly IPredictionInterface _predictionInterface;

    public PredictionService(IPredictionInterface predictionInterface)
    {
        _predictionInterface = predictionInterface;
    }

    public async Task AddPredictionAsync(Prediction prediction)
    {
        await _predictionInterface.AddPredictionAsync(prediction);
    }
}