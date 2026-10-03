using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftUmar_Virtuals.Infrastructure.Services.Logger.Slack
{
    public class SlackOptions
    {
        public string WebhookUrl { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Environment { get; set; } = string.Empty;
    }
}
