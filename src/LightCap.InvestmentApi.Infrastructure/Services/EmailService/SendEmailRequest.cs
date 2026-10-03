using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Infrastructure.Services.EmailService
{
    public class SendEmailRequest
    {
        public required string To { get; set; } 
        public required string Subject { get; set; } 
        public required string Body { get; set; } 
    }
}
