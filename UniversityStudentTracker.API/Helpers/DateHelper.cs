namespace UniversityStudentTracker.API.Helpers;

public class DateHelper
{
    public static (DateTime start, DateTime end) GetStartEndOfDay(DateTime date)
    {
        var startOfDay = date.Date;
        var endOfDay = date.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
        return (startOfDay, endOfDay);
    }
}