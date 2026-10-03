using System.Text;
using System.Web;

namespace LightCap.InvestmentApi.Application.Common.Helpers;

public static class TextHelper
{
    /// <summary>
    /// Ensures text is safely preserved with spaces, parentheses, and special characters intact.
    /// Optionally HTML-encodes if it's meant for web display.
    /// </summary>
    public static string PreserveFormatting(string input, bool forWebDisplay = false)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        // Use UTF-8 to safely preserve all symbols, accents, and spaces
        var utf8Bytes = Encoding.UTF8.GetBytes(input);
        var cleanText = Encoding.UTF8.GetString(utf8Bytes);

        // Optionally HTML-encode for web rendering
        if (forWebDisplay)
            cleanText = HttpUtility.HtmlEncode(cleanText);

        return cleanText;
    }

    public static string FormatAddress(string rawAddress)
    {
        if (string.IsNullOrWhiteSpace(rawAddress))
            return string.Empty;

        // Normalize spaces and commas
        string formatted = rawAddress
            .Replace(" ,", ",")                      // Remove space before commas
            .Replace("  ", " ")                      // Collapse double spaces
            .Trim();

        // Fix content inside parentheses if any
        int openIdx = formatted.IndexOf('(');
        int closeIdx = formatted.IndexOf(')');
        if (openIdx >= 0 && closeIdx > openIdx)
        {
            string inner = formatted.Substring(openIdx + 1, closeIdx - openIdx - 1);

            // Clean the inner content too
            string cleanedInner = string.Join(", ",
                inner.Split(',')
                     .Select(s => s.Trim())
                     .Where(s => !string.IsNullOrEmpty(s))
            );

            // Replace old inner section with the cleaned version
            formatted = formatted.Substring(0, openIdx + 1)
                        + cleanedInner
                        + formatted.Substring(closeIdx);
        }

        // Ensure space before the dash if it's joined too tightly
        formatted = System.Text.RegularExpressions.Regex.Replace(
            formatted,
            @"\)\s*-\s*",
            ") - "
        );

        // Trim any trailing/leading commas, spaces
        formatted = formatted.Trim(' ', ',');

        return formatted;
    }
}