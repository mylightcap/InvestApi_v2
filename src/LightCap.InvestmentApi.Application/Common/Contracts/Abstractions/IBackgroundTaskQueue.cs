namespace LightCap.InvestmentApi.Application.Common.Contracts.Abstractions;

public interface IBackgroundTaskQueue
{
    ValueTask QueueBackgroundWorkItemAsync(
        Func<IServiceProvider, CancellationToken, Task> workItem,
        string? correlationId = null,
        CancellationToken cancellationToken = default);

    Task<Func<IServiceProvider, CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
}