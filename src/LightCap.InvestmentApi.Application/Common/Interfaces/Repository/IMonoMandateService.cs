using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Application.Common.Interfaces.Repository
{
    public interface IMonoMandateService
    {
        Task<MonoCreateMandateResult> CreateMandateAsync(MonoCreateMandateRequest request, CancellationToken cancellationToken);
    }

    public class MonoCreateMandateRequest
    {
        public string MonoCustomerId { get; set; } = string.Empty;
        public string AccountId { get; set; } = string.Empty; // the linked account, from Mono Connect earlier
        public decimal MaxAmount { get; set; }                 // the maximum ever debitable, e.g. 50000 (₦50,000)
        public string Reference { get; set; } = string.Empty;
    }

    public class MonoCreateMandateResult
    {
        public bool Success { get; set; }
        public string? MonoMandateId { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
