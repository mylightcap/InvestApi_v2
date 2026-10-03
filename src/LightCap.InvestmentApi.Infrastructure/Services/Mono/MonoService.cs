using LightCap.InvestmentApi.Application.Common.Interfaces;
using LightCap.InvestmentApi.Application.Features.MonoService.MonoHandler.Command;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Infrastructure.Services.Mono
{
    public class MonoService : IMonoService
    {
        private readonly HttpClient _httpClient;
        private readonly string _secretKey;

        public MonoService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _secretKey = config["Mono:SecretKey"] ?? throw new InvalidOperationException("Mono:SecretKey is not configured.");
        } 

        public async Task<MonoExchangeTokenResult> ExchangeTokenAsync(string code, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.withmono.com/v2/accounts/auth");
            request.Headers.Add("mono-sec-key", _secretKey);
            request.Content = JsonContent.Create(new { code });

            var response = await _httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
                return new MonoExchangeTokenResult
                {
                    Success = false,
                    ErrorMessage = $"Mono exchange failed ({(int)response.StatusCode}): {errorBody}"
                };
            }

            var payload = await response.Content.ReadFromJsonAsync<MonoExchangeTokenApiResponse>(cancellationToken: cancellationToken);

            if (payload?.Id is null)
            {
                return new MonoExchangeTokenResult
                {
                    Success = false,
                    ErrorMessage = "Mono response did not contain an account id."
                };
            }

            return new MonoExchangeTokenResult
            {
                Success = true,
                AccountId = payload.Id
            };
        }

        // Shape of Mono's response - adjust field names if their actual response
        // wraps this in a "data" object; check the real payload during testing.
       

    }
}
