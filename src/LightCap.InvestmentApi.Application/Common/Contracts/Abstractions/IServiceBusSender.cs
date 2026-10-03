namespace LightCap.InvestmentApi.Application.Common.Contracts.Abstractions;

public interface IServiceBusSender
{
    Task SendMessageAsync<T>(T message, CancellationToken cancellationToken = default);
    Task SendMessagesAsync<T>(IEnumerable<T> messages, CancellationToken cancellationToken = default);
}