using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Application.Features.MonoService.Transaction.Commands
{
    public class MonoTransactionWebhookData
    {
        public string Id { get; set; } = string.Empty;           // Mono's transaction ID - used as our idempotency key
        public string Account { get; set; } = string.Empty;      // the linked Mono account ID this transaction belongs to
        public decimal Amount { get; set; }                       // amount in kobo, per Mono convention
        public string Type { get; set; } = string.Empty;         // "debit" or "credit"
        public string Narration { get; set; } = string.Empty;
    }
}
