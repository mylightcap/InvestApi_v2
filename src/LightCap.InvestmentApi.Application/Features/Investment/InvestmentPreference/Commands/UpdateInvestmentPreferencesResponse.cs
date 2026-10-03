using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Application.Features.Investment.InvestmentPreference.Commands
{
    public class UpdateInvestmentPreferencesResponse
    {
        public decimal AutoInvestPercentage { get; set; }
        public string InvestmentMode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
