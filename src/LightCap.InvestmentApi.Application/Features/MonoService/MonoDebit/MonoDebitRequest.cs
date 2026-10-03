using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Application.Features.MonoService.MonoDebit
{
    public class MonoDebitRequest
    {
        public string MonoMandateId { get; set; } = string.Empty;
        public decimal Amount { get; set; } // in Naira - converted to kobo inside the service
        public string Reference { get; set; } = string.Empty;
        public string Narration { get; set; } = string.Empty;
    }
}
