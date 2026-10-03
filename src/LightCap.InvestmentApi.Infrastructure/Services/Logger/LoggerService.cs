using LightCap.InvestmentApi.Application.Common.Interfaces;
using LightCap.InvestmentApi.Infrastructure.Services.Logger.Slack;
using SoftUmar_Virtuals.Infrastructure.Services.Logger.Slack;
using System.Text.Json;

namespace LightCap.InvestmentApi.Infrastructure.Services.Logger
{
    public class LoggerService : ILoggerService
    {
        private readonly ISlackLogger _slackLogger;


        public LoggerService(ISlackLogger slackLogger)
        {
            _slackLogger = slackLogger;
        }


        public async Task LogInfo(string message, string methodName, string? traceId = null)
        {
            await LogToAll("INFO", message, methodName, traceId);
        }


        public async Task LogWarning(string message, string methodName, string? traceId = null)
        {
            await LogToAll("WARNING", message, methodName, traceId);
        }


        public async Task LogError(string message, string methodName, Exception? ex = null, string? traceId = null)
        {
            var fullMessage = ex != null ? message + $", Exception: {ex.Message}" : message;
            await LogToAll("ERROR", fullMessage, methodName, traceId);
        }


        public async Task LogRequestResponse(string requestJson, string responseJson, string methodName, bool success, string? traceId = null)
        {
            var logMessage = JsonSerializer.Serialize(new
            {
                TraceId = traceId ?? Guid.NewGuid().ToString(),
                Method = methodName,
                Request = requestJson,
                Response = responseJson,
                Success = success,
                Timestamp = DateTime.UtcNow
            });


            await LogToConsole(logMessage);
            _slackLogger.Log(logMessage, success);
        }


        private static async Task LogToConsole(string message)
        {
            await Console.Out.WriteLineAsync("=== === >>> " + message);
        }


        private async Task LogToAll(string level, string message, string methodName, string? traceId)
        {
            var logObject = JsonSerializer.Serialize(new
            {
                Level = level,
                TraceId = traceId ?? Guid.NewGuid().ToString(),
                Method = methodName,
                Message = message,
                Timestamp = DateTime.UtcNow
            });


            await LogToConsole(logObject);
            _slackLogger.Log(logObject, level == "INFO");
        }
    }
}
