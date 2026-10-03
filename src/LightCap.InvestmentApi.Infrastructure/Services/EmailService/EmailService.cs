using LightCap.InvestmentApi.Application.Common.Email;
using LightCap.InvestmentApi.Application.Common.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Net.Mail;

namespace LightCap.InvestmentApi.Infrastructure.Services.EmailService;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<EmailResponse> SendEmailWithFallback(
        string to,
        string subject,
        string body,
        bool isHtml = true)
    {
        try
         {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(
                _configuration["Brevo:FromName"],
                _configuration["Brevo:FromEmail"]
            ));

            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            email.Body = new TextPart(isHtml ? "html" : "plain")
            {
                Text = body
            };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            await smtp.ConnectAsync(
                _configuration["Brevo:Host"],
                int.Parse(_configuration["Brevo:Port"]!),
                MailKit.Security.SecureSocketOptions.StartTls
            );

            await smtp.AuthenticateAsync(
                _configuration["Brevo:Username"],
                _configuration["Brevo:Password"]
            );

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            return new EmailResponse
            {
                Success = true,
                Message = "Email sent successfully via Brevo"
            };
        }
        catch (Exception ex)
        {
            return new EmailResponse
            {
                Success = false,
                Message = $"Email sending failed: {ex.Message}"
            };
        }
    }
}