using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Application.Features.MonoService.MonoHandler.Command
{
    public class MonoExchangeTokenResponse
    {
        public string AccountId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
