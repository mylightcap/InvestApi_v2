namespace CustOps.Application.Common.Contracts.Abstractions;

public interface IAuthHeaderProvider
{
    Task<(string Name, string Value)> GetAsync(CancellationToken cancellationToken = default);
}