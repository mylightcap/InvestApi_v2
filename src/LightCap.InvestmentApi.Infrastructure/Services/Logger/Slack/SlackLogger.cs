using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SoftUmar_Virtuals.Infrastructure.Services.Logger.Slack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Infrastructure.Services.Logger.Slack
{
    public class SlackLogger(IOptions<SlackOptions> slackOptions) : ISlackLogger
    {
        public async Task Log(string message, bool successful)
        {
            var payload = new
            {
                channel = $"{slackOptions.Value.Channel}_{slackOptions.Value.Environment}",
                username = slackOptions.Value.Username,
                text = successful ? $"{message}" : $"`{message}`",
                successful
            };

            var msg = JsonConvert.SerializeObject(payload);
            await Task.Run(() => { Console.WriteLine(msg); });
        }
    }
}
