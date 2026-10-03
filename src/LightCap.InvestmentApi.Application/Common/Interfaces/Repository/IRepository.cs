using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Application.Common.Interfaces
{
    
  public interface IRepository<T> where T : class
    {
        DbSet<T> Entity { get; }                 // NEW
        DatabaseFacade Database { get; }

        Task<T?> GetByIdAsync(object id);
        Task<IEnumerable<T>> GetAllAsync();
        IEnumerable<T> GetAll(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity, CancellationToken cancellationToken);
        Task<int> SaveChanges(CancellationToken cancellationToken);
        Task UpdateAsync(T entity);
        bool Exists(Expression<Func<T, bool>> expression);
        Task DeleteAsync(T entity);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Guid id);
        IQueryable<T> GetQueryable();
        Task<T> GetRecentRecord<TOrderBy>(Expression<Func<T, bool>> expression, Expression<Func<T, TOrderBy>> orderBy);
        IQueryable<T> GetAndInclude(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includeProperties);
        Task<T> Insert(T entity);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> expression);
        Task<(IEnumerable<T> Data, int Total)> GetPaginatedAsync(int page, int pageSize, params Expression<Func<T, object>>[] includes);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
    }
}
