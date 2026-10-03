using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Application.Features.MonoService.MonoDebit.MonoDebitWebhook.Commands
{
    public class MonoDebitWebhookData
    {
        public string Reference { get; set; } = string.Empty; // matches MonoDebitReference we saved earlier
        public string Status { get; set; } = string.Empty;
    }
}
