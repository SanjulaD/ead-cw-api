using Python.Runtime;
using UniversityStudentTracker.API.Models.Domains;
using UniversityStudentTracker.API.Models.DTO.Prediction;
using UniversityStudentTracker.API.Repositories;

namespace UniversityStudentTracker.API.Services;

public class PredictionService
{
    private readonly IPredictionInterface _predictionRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public PredictionService(IPredictionInterface predictionRepository, IWebHostEnvironment webHostEnvironment)
    {
        _predictionRepository = predictionRepository;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<PredictionResultDto> PredictAsync(string subject)
    {
        // Fetch study sessions and breaks for the user and subject
        var studySessions = await _predictionRepository.GetStudySessionsByUserAndSubjectAsync(subject);
        var breaks = await _predictionRepository.GetBreaksByUserAsync();

        if (!studySessions.Any())
            throw new InvalidOperationException("No study sessions found for the user and subject.");

        var averageStudyDuration = (float)studySessions.Average(ss => ss.DurationMinutes);

        var input = new PredictionDto
        {
            Subject = subject,
            AverageStudyTime = averageStudyDuration,
            AverageBreakTime = (float)breaks.Average(b => b.DurationMinutes)
        };

        var modelPath = Path.Combine(_webHostEnvironment.ContentRootPath, "ML", "model.zip");

        // Load the model and make a prediction
        var predictionResult = Predict(modelPath, input);

        // Save the prediction result to the database
        var prediction = new Prediction
        {
            Subject = subject,
            PredictedGrade = predictionResult.PredictedGrade,
            PredictedKnowledgeLevel = predictionResult.PredictedKnowledgeLevel,
            PredictionDate = DateTime.UtcNow
        };

        await _predictionRepository.CreatePredictionAsync(prediction);

        return predictionResult;
    }

    private static PredictionResultDto Predict(string modelPath, PredictionDto input)
    {
        Runtime.PythonDLL = @"/Users/sanjuladealwis/anaconda3/bin/python";

        // Initialize Python runtime (ensure Python.Runtime is properly loaded)
        PythonEngine.Initialize();

        // Python code as string
        var pythonCode = @"
                    import pickle

                    def predict(input_data):
                        # Load the model from trained_model.pkl
                        with open('trained_model.pkl', 'rb') as f:
                            model = pickle.load(f)
                        
                        # Perform prediction using input_data
                        prediction = model.predict(input_data)

                        # Return prediction result
                        return prediction
                    ";

        // Execute the Python code
        using (Py.GIL())
        {
            dynamic pickle = Py.Import("pickle");

            // Load the model from the pickle file
            dynamic model;
            using (var fs = new FileStream(modelPath, FileMode.Open, FileAccess.Read))
            using (var reader = new BinaryReader(fs))
            {
                var modelBytes = reader.ReadBytes((int)fs.Length);
                model = pickle.loads(modelBytes);
            }

            // Prepare input data for prediction
            dynamic inputData = new PyDict();
            inputData["Subject"] = input.Subject;
            inputData["AverageStudyTime"] = input.AverageStudyTime;
            inputData["AverageBreakTime"] = input.AverageBreakTime;

            // Perform prediction
            var prediction = model.predict(inputData); // Adjust based on your model's predict method

            // Convert Python prediction to C# object
            var predictionResult = new PredictionResultDto
            {
                PredictedGrade = (float)prediction["predictedGrade"], // Adjust field names and types
                PredictedKnowledgeLevel = (int)prediction["predictedKnowledgeLevel"]
            };

            return predictionResult;
        }
    }
}