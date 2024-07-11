namespace UniversityStudentTracker.API.Helpers;

public abstract class TimeHelper
{
    public static int[] ConvertMinutesToHours(int[] minutes)
    {
        return minutes.Select(m => m / 60).ToArray();
    }
}