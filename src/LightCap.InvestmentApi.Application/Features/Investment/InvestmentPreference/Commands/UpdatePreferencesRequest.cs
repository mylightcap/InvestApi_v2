using LightCap.InvestmentApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Application.Features.Investment.InvestmentPreference.Commands
{
    public record UpdatePreferencesRequest(decimal AutoInvestPercentage, InvestmentMode InvestmentMode);
}
