using LightCap.InvestmentApi.Application.Common.Interfaces.Repository;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Infrastructure.Services.Mono
{
    public class MonoMandateService : IMonoMandateService
    {
        private readonly HttpClient _httpClient;
        private readonly string _secretKey;

        public MonoMandateService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _secretKey = config["Mono:SecretKey"]
                ?? throw new InvalidOperationException("Mono:SecretKey is not configured.");
        }

        public async Task<MonoCreateMandateResult> CreateMandateAsync(MonoCreateMandateRequest request, CancellationToken cancellationToken)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://api.withmono.com/v2/payments/initiate");
            httpRequest.Headers.Add("mono-sec-key", _secretKey);

            httpRequest.Content = JsonContent.Create(new
            {
                type = "recurring-debit",
                amount = (long)(request.MaxAmount * 100), // kobo
                description = "LightCap round-up investment mandate",
                reference = request.Reference,
                customer = request.MonoCustomerId,
                account = request.AccountId,
                mandate_type = "emandate",
                debit_type = "variable"
            });

            var response = await _httpClient.SendAsync(httpRequest, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
                return new MonoCreateMandateResult
                {
                    Success = false,
                    ErrorMessage = $"Mono mandate creation failed ({(int)response.StatusCode}): {errorBody}"
                };
            }

            // NOTE: confirm this response shape against a real sandbox call -
            // same caution flagged for exchange-token and customer creation.
            var payload = await response.Content.ReadFromJsonAsync<MonoCreateMandateApiResponse>(cancellationToken: cancellationToken);
            var mandateId = payload?.Data?.Id ?? payload?.Id;

            if (string.IsNullOrEmpty(mandateId))
            {
                return new MonoCreateMandateResult
                {
                    Success = false,
                    ErrorMessage = "Mono response did not contain a mandate id."
                };
            }

            return new MonoCreateMandateResult
            {
                Success = true,
                MonoMandateId = mandateId
            };
        }

        private class MonoCreateMandateApiResponse
        {
            public string? Id { get; set; }
            public MonoMandateData? Data { get; set; }
        }

        private class MonoMandateData
        {
            public string? Id { get; set; }
        }
    }
}
