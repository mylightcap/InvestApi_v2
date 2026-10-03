using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Application.Common.Interfaces
{
    public interface ISmsService
    {
        Task<SmsSendResult> SendSmsAsync(string phoneNumber, string message, CancellationToken cancellationToken);
    }

   
}
