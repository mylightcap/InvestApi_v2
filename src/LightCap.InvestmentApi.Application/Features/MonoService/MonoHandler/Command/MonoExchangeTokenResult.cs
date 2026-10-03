using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Application.Features.MonoService.MonoHandler.Command
{
    public class MonoExchangeTokenResult
    {
        public bool Success { get; set; }
        public string? AccountId { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
