namespace LightCap.InvestmentApi.Application.Common.Contracts.Abstractions;

public interface IUnitOfWork
{ 
    Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> operation, CancellationToken ct = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    // Raw execution (EF Core or Dapper)
    Task<int> ExecuteAsync(string sql, object? parameters = null, CancellationToken ct = default);

    // Fast querying (Dapper)
    Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? parameters = null, CancellationToken ct = default);

    Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? parameters = null, CancellationToken ct = default);
}