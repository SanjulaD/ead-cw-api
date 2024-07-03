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

    public async Task<Prediction> CreatePredictionAsync(Prediction prediction)
    {
        prediction.UserID = _userInterface.GetUserId();
        await _studentPerformance.Predictions.AddAsync(prediction);
        await _studentPerformance.SaveChangesAsync();
        return prediction;
    }

    public async Task<List<StudySession>> GetStudySessionsByUserAndSubjectAsync(string subject)
    {
        var userId = _userInterface.GetUserId();
        return await _studentPerformance.StudySessions
            .Where(ss => ss.UserID == userId && ss.Subject == subject)
            .ToListAsync();
    }

    public async Task<List<Break>> GetBreaksByUserAsync()
    {
        var userId = _userInterface.GetUserId();
        return await _studentPerformance.Breaks
            .Where(b => b.UserID == userId)
            .ToListAsync();
    }
}