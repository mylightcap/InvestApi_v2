using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace LightCap.InvestmentApi.Application.Common.Contracts.Abstractions.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> GetAll();
    IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter);

    public IQueryable<TResult> GetAllV2<TResult>(
    Expression<Func<TEntity, bool>> filter,
    Expression<Func<TEntity, TResult>> selector);
	Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken = default);

    Task<TResult?> GetRecentRecordAsync<TOrderBy, TResult>(
        Expression<Func<TEntity, bool>>? filter = null,
        Expression<Func<TEntity, TOrderBy>>? orderBy = null,
        Expression<Func<TEntity, TResult>>? selector = null,
        bool descending = true,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<TEntity> StreamAsync(
        Expression<Func<TEntity, bool>> filter, [EnumeratorCancellation] CancellationToken cancellationToken = default);

    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task<int> UpdateAsync(TEntity entity);

    Task<bool> ExistsAsync(object id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(
    Expression<Func<TEntity, bool>> predicate,
    CancellationToken ct = default);

    void Delete(TEntity entity);
    Task<T?> FirstOrDefaultAsync<T>(
    Expression<Func<T, bool>> predicate,
    CancellationToken cancellationToken = default)
    where T : class;
}