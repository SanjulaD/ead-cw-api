using Microsoft.EntityFrameworkCore;
using UniversityStudentTracker.API.Contexts;
using UniversityStudentTracker.API.Models.Domains;

namespace UniversityStudentTracker.API.Repositories;

public class PredictionRepository : IPredictionInterface
{
    private readonly StudentPerformance _studentPerformance;
    private readonly IUserInterface _userInterface;

    public PredictionRepository(StudentPerformance studentPerformance, IUserInterface userInterface)
    {
        _studentPerformance = studentPerformance;
        _userInterface = userInterface;
    }

    public async Task AddPredictionAsync(Prediction prediction)
    {
        prediction.UserID = _userInterface.GetUserId();
        _studentPerformance.Add(prediction);
        await _studentPerformance.SaveChangesAsync();
    }

    public async Task<List<Prediction>> GetAllAsync()
    {
        var userId = _userInterface.GetUserId();

        return await _studentPerformance.Predictions
            .Where(b => b.UserID == userId)
            .ToListAsync();
    }
}