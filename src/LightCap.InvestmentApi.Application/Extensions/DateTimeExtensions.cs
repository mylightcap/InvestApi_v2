namespace LightCap.InvestmentApi.Application.Extensions;

public static class DateTimeExtensions
{
    public static DateTime EnsureRange(this DateTime startDate, DateTime endDate)
        => startDate == endDate ? endDate.AddDays(1) : endDate;
}