using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Application.Features.MonoService.Transaction.Commands
{
    public class MonoTransactionWebhookPayload
    {
        public string Event { get; set; } = string.Empty;
        public MonoTransactionWebhookData Data { get; set; } = new();
    }
}
