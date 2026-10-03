using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Application.Features.MonoService.MonoMandate
{
    public class MonoMandateWebhookData
    {
        public string Id { get; set; } = string.Empty; // the mandate id
        public string Status { get; set; } = string.Empty;
    }
}
