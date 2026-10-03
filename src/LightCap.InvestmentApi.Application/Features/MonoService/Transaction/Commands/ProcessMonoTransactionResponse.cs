using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Application.Features.MonoService.Transaction.Commands
{
    public class ProcessMonoTransactionResponse
    {
        public bool RoundUpApplied { get; set; }
        public bool ThresholdReached { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
