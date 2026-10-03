using LightCap.InvestmentApi.Application.Common;
using LightCap.InvestmentApi.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace LightCap.InvestmentApi.Infrastructure.Services.Sms
{
    public class SmsService : ISmsService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _senderId;

        public SmsService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["Brevo:ApiKey"]
                ?? throw new InvalidOperationException("Brevo:ApiKey is not configured.");
            _senderId = config["Brevo:SmsSenderId"]
                ?? throw new InvalidOperationException("Brevo:SmsSenderId is not configured.");
        }

        public async Task<SmsSendResult> SendSmsAsync(string phoneNumber, string message, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.brevo.com/v3/transactionalSMS/send");
            request.Headers.Add("api-key", _apiKey);
            request.Headers.Add("accept", "application/json");

            request.Content = JsonContent.Create(new
            {
                sender = _senderId,          // e.g. "LightCap" - must be pre-approved in your Brevo dashboard
                recipient = phoneNumber,      // international format, e.g. "234XXXXXXXXXX" - no leading +/00
                content = message,
                type = "transactional"        // NOT "marketing" - this is an OTP, not a promo message
            });

            var response = await _httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
                return new SmsSendResult
                {
                    Success = false,
                    ErrorMessage = $"Brevo SMS send failed ({(int)response.StatusCode}): {errorBody}"
                };
            }

            return new SmsSendResult { Success = true };
        }
    }
}