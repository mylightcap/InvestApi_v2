using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Application.Features.MonoService.MonoHandler.Command
{
    public class MonoExchangeTokenApiResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public MonoExchangeTokenData? Data { get; set; }
    }

    public class MonoExchangeTokenData
    {
        public string Token { get; set; } = string.Empty;
    }
}
