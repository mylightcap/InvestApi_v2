namespace LightCap.InvestmentApi.Infrastructure.Persistence;
//UNCOMMENT WHEN USING DAPPER FOR RAW SQL EXECUTION IN THE UNIT OF WORK
//public class UnitOfWork(AppDbContext context) : IUnitOfWork
//{ 
//    private DbConnection Connection => context.Database.GetDbConnection();

//	public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
//        => await context.Database.BeginTransactionAsync(cancellationToken);

//    public async Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> operation,
//        CancellationToken ct = default)
//    {
//        var strategy = context.Database.CreateExecutionStrategy();

//        return await strategy.ExecuteAsync(async () =>
//        {
//            // Start a new transaction inside the execution strategy delegate
//            await using var tx = await context.Database.BeginTransactionAsync(ct);
//            try
//            {
//                var result = await operation();

//                // Persist changes if not already saved by the operation
//                await context.SaveChangesAsync(ct);

//                await tx.CommitAsync(ct);
//                return result;
//            }
//            catch
//            {
//                try
//                {
//                    await tx.RollbackAsync(ct);
//                }
//                catch
//                {
//                    /* swallow or log rollback failure */
//                }

//                throw;
//            }
//        });
//    }

//    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
//        => await context.Database.CommitTransactionAsync(cancellationToken);

//    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
//        => await context.Database.RollbackTransactionAsync(cancellationToken);

//    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
//        => await context.SaveChangesAsync(cancellationToken);

//    public async Task<int> ExecuteAsync(string sql, object? parameters = null, CancellationToken ct = default)
//    {
//        if (string.IsNullOrWhiteSpace(sql))
//            throw new ArgumentException("SQL cannot be null or empty.", nameof(sql));

//        await EnsureConnectionOpenAsync(ct);

//        return await Connection.ExecuteAsync(new CommandDefinition(
//            sql,
//            parameters,
//            transaction: context.Database.CurrentTransaction?.GetDbTransaction(),
//            cancellationToken: ct));
//    }

//    public async Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? parameters = null, CancellationToken ct = default)
//    {
//        await EnsureConnectionOpenAsync(ct);

//        return await Connection.QuerySingleOrDefaultAsync<T>(new CommandDefinition(
//            sql,
//            parameters,
//            transaction: context.Database.CurrentTransaction?.GetDbTransaction(),
//            cancellationToken: ct));
//    }
//    private async Task EnsureConnectionOpenAsync(CancellationToken ct = default)
//    {
//        if (context.Database.GetDbConnection().State != System.Data.ConnectionState.Open)
//        {
//            await context.Database.GetDbConnection().OpenAsync(ct);
//        }
//    }

//    public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? parameters = null, CancellationToken ct = default)
//    {
//        await EnsureConnectionOpenAsync(ct);

//        var result = await Connection.QueryAsync<T>(new CommandDefinition(
//            sql,
//            parameters,
//            transaction: context.Database.CurrentTransaction?.GetDbTransaction(),
//            cancellationToken: ct));

//        return result.AsList();
//    }
//}