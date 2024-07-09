namespace UniversityStudentTracker.API.Helpers;

public class TimeHelper
{
    public static int[] ConvertMinutesToHours(int[] minutesArray)
    {
        var hoursArray = new int[minutesArray.Length];
        for (var i = 0; i < minutesArray.Length; i++) hoursArray[i] = minutesArray[i] / 60;

        return hoursArray;
    }
}