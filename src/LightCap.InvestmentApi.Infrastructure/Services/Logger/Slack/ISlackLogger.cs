using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Infrastructure.Services.Logger.Slack
{
    public interface ISlackLogger
    {
        Task Log(string message, bool successful);
    }
}
