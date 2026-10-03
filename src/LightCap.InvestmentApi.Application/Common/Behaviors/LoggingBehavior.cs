using FluentResults;
using LightCap.InvestmentApi.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Text.Json;

namespace LightCap.InvestmentApi.Application.Common.Behaviors;

    public class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        private readonly ILoggerService _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoggingBehavior(
            ILoggerService logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var methodName = typeof(TRequest).Name;
            var traceId = _httpContextAccessor.HttpContext?.TraceIdentifier
                          ?? Guid.NewGuid().ToString();

            var requestJson = JsonSerializer.Serialize(request);

            TResponse response;
            var sw = Stopwatch.StartNew();

            try
            {
                response = await next();
                sw.Stop();

                string responseJson;

                if (response is IResultBase result)
                {
                    if (result.IsSuccess)
                    {
                        object value = result;
                        var valueProperty = result.GetType().GetProperty("Value");

                        if (valueProperty != null)
                            value = valueProperty.GetValue(result);

                        responseJson = JsonSerializer.Serialize(new
                        {
                            Value = value
                        });
                    }
                    else
                    {
                        responseJson = JsonSerializer.Serialize(new
                        {
                            Errors = result.Errors.Select(e => e.Message)
                        });
                    }
                }
                else
                {
                    responseJson = JsonSerializer.Serialize(response);
                }

                await _logger.LogRequestResponse(
                    requestJson,
                    responseJson,
                    methodName,
                    true,
                    traceId);
            }
            catch (Exception ex)
            {
                sw.Stop();

                await _logger.LogError(
                    "CQRS Handler Exception",
                    methodName,
                    ex,
                    traceId);

                throw;
            }

            return response;
        }
    }

