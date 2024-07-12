namespace UniversityStudentTracker.API.Helpers;

public abstract class DateHelper
{
    public static (DateTime start, DateTime end) GetStartEndOfDay(DateTime date)
    {
        var startOfDay = date.Date;
        var endOfDay = date.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
        return (startOfDay, endOfDay);
    }

    public static (DateTime start, DateTime end) GetStartEndOfWeek(DateTime date)
    {
        var dayOfWeek = (int)date.DayOfWeek;
        var startOfWeek = date.Date.AddDays(-dayOfWeek);
        var endOfWeek = startOfWeek.AddDays(6).AddHours(23).AddMinutes(59).AddSeconds(59);
        return (startOfWeek, endOfWeek);
    }

    public static (DateTime start, DateTime end) GetStartEndOfMonth(DateTime date)
    {
        var startOfMonth = new DateTime(date.Year, date.Month, 1);
        var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
        return (startOfMonth, endOfMonth);
    }

    public static (DateTime start, DateTime end) GetStartEndOfYear(DateTime date)
    {
        var startOfYear = new DateTime(date.Year, 1, 1);
        var endOfYear = new DateTime(date.Year, 12, 31, 23, 59, 59);
        return (startOfYear, endOfYear);
    }
}