using LightCap.InvestmentApi.Application.Common.Email;

namespace LightCap.InvestmentApi.Application.Common.Interfaces;

public interface IEmailService
    {
    Task<EmailResponse> SendEmailWithFallback(string to, string subject, string body, bool isHtml = true);

    
}

