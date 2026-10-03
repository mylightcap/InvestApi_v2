using LightCap.InvestmentApi.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Application.Features.MonoService.MonoDebit
{
    public class MonoDebitService : IMonoDebitService
    {
        private readonly HttpClient _httpClient;
        private readonly string _secretKey;

        // Mono's documented hard floor for Direct Debit - anything below this
        // is rejected by their API. Kept here as a safety net; the handler
        // calling this service should already be applying the ₦200 floor
        // before it ever reaches here, but this is a last line of defence.
        public const decimal MinimumDebitAmount = 200m;

        public MonoDebitService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _secretKey = config["Mono:SecretKey"]
                ?? throw new InvalidOperationException("Mono:SecretKey is not configured.");
        }

        public async Task<MonoDebitResult> DebitAsync(MonoDebitRequest request, CancellationToken cancellationToken)
        {
            if (request.Amount < MinimumDebitAmount)
            {
                return new MonoDebitResult
                {
                    Success = false,
                    ErrorMessage = $"Debit amount \u20a6{request.Amount} is below Mono's \u20a6{MinimumDebitAmount} minimum."
                };
            }

            var url = $"https://api.withmono.com/v3/payments/mandates/{request.MonoMandateId}/debit";
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Headers.Add("mono-sec-key", _secretKey);

            httpRequest.Content = JsonContent.Create(new
            {
                amount = (long)(request.Amount * 100), // kobo
                reference = request.Reference,
                narration = request.Narration
            });

            var response = await _httpClient.SendAsync(httpRequest, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
                return new MonoDebitResult
                {
                    Success = false,
                    ErrorMessage = $"Mono debit call failed ({(int)response.StatusCode}): {errorBody}"
                };
            }

            // The debit call succeeding here means Mono ACCEPTED the request -
            // it does NOT mean the money has landed yet. Final confirmation
            // comes later via a separate webhook (built in the next step).
            return new MonoDebitResult { Success = true };
        }
    }
}
