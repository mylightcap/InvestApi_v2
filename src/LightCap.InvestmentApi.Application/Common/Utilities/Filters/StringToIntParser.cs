using System.Text.RegularExpressions;

namespace LightCap.InvestmentApi.Application.Common.Utilities.Filters;

public class StringToIntParser
{
    public static int ExtractTatValue(string? tatValue)
    {
        if (string.IsNullOrWhiteSpace(tatValue))
            return 0;

        var match = Regex.Match(tatValue, @"\d+");
        return match.Success ? int.Parse(match.Value) : 0;
    }

    public static int ExtractTurnaroundInt(string tat)
    {
        var number = new string(tat.TakeWhile(char.IsDigit).ToArray());
        return int.TryParse(number, out int result) ? result : 0;
    }

    public static string ExtractTurnaroundUnit(string tat)
    {
        return new string(tat.SkipWhile(char.IsDigit).ToArray()).Trim();
    }
}