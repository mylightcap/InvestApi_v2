using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Application.Features.MonoService.MonoDebit.MonoDebitWebhook.Commands
{
    public class MonoDebitWebhookPayload
    {
        public string Event { get; set; } = string.Empty; // e.g. "direct_debit.payment_successful" / "direct_debit.payment_failed"
        public MonoDebitWebhookData Data { get; set; } = new();
    }
}
