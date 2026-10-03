using System.Text.Json;

namespace LightCap.InvestmentApi.Application.Extensions;

    public static class UtilitesExtensions
    {
		// to json static method.
		public static string ToJson(this object obj)
		{
			if (obj == null) return string.Empty;	
			return JsonSerializer.Serialize(obj);
		}

    public static string GetExtensionFromContentType(string contentType)
    {
        return contentType.ToLower() switch
        {
            "image/jpeg" => ".jpg",
            "image/png" => ".png",
            "video/mp4" => ".mp4",
            _ => ".bin"
        };
    }

    public static DateTime? ParseDate(string? value)
    {
        if (DateTime.TryParse(value, out var date))
            return date;

        return null;
    }

}