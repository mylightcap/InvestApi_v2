namespace LightCap.InvestmentApi.Application.Common.Contracts.Abstractions;

public interface IMassTransitAzureServiceBusClient
{
    Task Send<T>(T message, CancellationToken cancellationToken = default);
    void Sendv2<T>(T message, CancellationToken cancellationToken = default);
    Task Sendv3<T>(T message, CancellationToken cancellationToken = default);
}