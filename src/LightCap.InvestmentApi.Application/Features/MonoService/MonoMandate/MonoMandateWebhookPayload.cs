using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Application.Features.MonoService.MonoMandate
{
    public class MonoMandateWebhookPayload
    {
        public string Event { get; set; } = string.Empty; // e.g. "mandate.ready_to_debit"
        public MonoMandateWebhookData Data { get; set; } = new();
    }
}
