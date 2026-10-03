using CustOps.Application.Common.Contracts.Abstractions;
using System.Net.Http.Headers;

namespace LightCap.InvestmentApi.Infrastructure.Services.Integrations.Auth;

public class AuthHeaderHandler<TProvider>(TProvider provider) : DelegatingHandler
    where TProvider : IAuthHeaderProvider
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var (name, value) = await provider.GetAsync(cancellationToken);
        if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(value))
        {
            if (name.Equals("Authorization", StringComparison.OrdinalIgnoreCase))
            {
                var parts = value.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                    request.Headers.Authorization = new AuthenticationHeaderValue(parts[0], parts[1]);
                else
                    request.Headers.Add(name, value);
            }
            else
                request.Headers.Add(name, value);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}