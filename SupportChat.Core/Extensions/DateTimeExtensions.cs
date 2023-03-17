namespace SupportChat.Core.Extensions;

public static class DateTimeExtensions
{
    public static DateTime ToNextMonthStart(this DateTime dateTime)
    {
        return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1);
    }
}