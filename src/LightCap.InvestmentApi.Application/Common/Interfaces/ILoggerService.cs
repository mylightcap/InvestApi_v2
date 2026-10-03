using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Application.Common.Interfaces
{
    public interface ILoggerService
    {
        //Task Log(string request, string response, string methodName, bool success = false);

        Task LogInfo(string message, string methodName, string? traceId = null);
        Task LogWarning(string message, string methodName, string? traceId = null);
        Task LogError(string message, string methodName, Exception? ex = null, string? traceId = null);
        Task LogRequestResponse(string requestJson, string responseJson, string methodName, bool success, string? traceId = null);
    }
}
