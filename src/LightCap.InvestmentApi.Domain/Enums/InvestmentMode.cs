using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Domain.Enums
{
    public enum InvestmentMode
    {
        UserDirected,   // System waits for the user to manually approve each investment
        AutoInvest      // System invests automatically once the threshold is reached
    }
}
