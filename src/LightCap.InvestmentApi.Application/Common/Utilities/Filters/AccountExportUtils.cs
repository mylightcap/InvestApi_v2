using System.Text;

namespace LightCap.InvestmentApi.Application.Common.Utilities.Filters;


    public static class AccountExportUtils
    {
        public static (byte[] Bytes, string FileName, string ContentType) BuildAccountsCsv(
            IEnumerable<string> accounts,
            string category,
            string filePrefix = "accounts")
        {
            var cleaned = accounts
                .Where(a => !string.IsNullOrWhiteSpace(a))
                .Select(a => a.Trim())
                .ToList();

            var cat = (category ?? "export").Trim().ToLowerInvariant();

            var sb = new StringBuilder();
            sb.AppendLine("AccountNumber,Category");

            foreach (var acc in cleaned)
            {
                var safe = acc.Replace("\"", "\"\"");
                sb.Append('"').Append(safe).Append('"').Append(',');
                sb.Append('"').Append(cat).Append('"').AppendLine();
            }

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            var fileName = $"{filePrefix}_{cat}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv";
            const string contentType = "text/csv; charset=utf-8";

            return (bytes, fileName, contentType);
        }
    }



