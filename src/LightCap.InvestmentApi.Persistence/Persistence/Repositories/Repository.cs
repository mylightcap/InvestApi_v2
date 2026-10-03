
using LightCap.InvestmentApi.Application.Common.Interfaces;
using LightCap.InvestmentApi.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace CustOps.Infrastructure.Persistence.Repositories;

//NOTE UNCOMMENT WHEN NEEDED, THIS IS A GENERIC REPOSITORY TEMPLATE FOR FUTURE USE
public class Repository<T> : IRepository<T>
 where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public DbSet<T> Entity => _dbSet;
    public DatabaseFacade Database => _context.Database;

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public async Task<T?> GetByIdAsync(object id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

   

    public async Task<(IEnumerable<T> Data, int Total)> GetPaginatedAsync(int page, int pageSize,
    params Expression<Func<T, object>>[] includes)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;

        var skipPage = (page - 1) * pageSize;

        IQueryable<T> query = _dbSet.AsNoTracking();

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        var total = await query.CountAsync();

        var data = await query
            .Skip(skipPage)
            .Take(pageSize)
            .ToListAsync();

        return (data, total);
    }


    public IEnumerable<T> GetAll(Expression<Func<T, bool>> expression)
    {
        return _dbSet.Where(expression).ToList();
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.Where(expression).ToListAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await Task.CompletedTask; // For consistency with async pattern
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await Task.CompletedTask; // For consistency with async pattern
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(predicate, cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _dbSet.FindAsync(id) != null;
    }
    public bool Exists(Expression<Func<T, bool>> expression)
    {
        return _dbSet.Where(expression).Any();
    }

    public async Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize)
    {
        return await _dbSet.AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<T> GetRecentRecord<TOrderBy>(Expression<Func<T, bool>> expression, Expression<Func<T, TOrderBy>> orderBy)
    {
        var record = await _dbSet.AsNoTracking().Where(expression).OrderByDescending(orderBy).FirstOrDefaultAsync();
        if (record == null)
        {
            return default(T)!; // Return default value for TEntity if no record found
        }
        return record;
    }

    public IQueryable<T> GetQueryable()
    {
        return _dbSet.AsQueryable();
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public async Task<int> SaveChanges(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync();
    }

    public IQueryable<T> GetAndInclude(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _dbSet;
        if (includeProperties != null)
        {
            query = includeProperties.Aggregate(query,
                (current, include) => current.Include(include));
        }

        return query;
    }
    public async Task<T> Insert(T entity)
    {
        await _dbSet.AddAsync(entity);

        if (await Save() > 0)
        {
            return entity;
        }
        else
        {
            return default(T)!;
        }
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(
    CancellationToken cancellationToken = default)
    {
        return await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public IEnumerable<T> GetAll()
    {
        return _dbSet.ToList();
    }

    private async Task<int> Save()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<T> GetSingleAsync(Expression<Func<T, bool>> expression)
    {/**Has not worked yet*/
        var item = await _dbSet.FirstOrDefaultAsync(expression);
        if (item == null)
            return default(T)!;

        return item;
    }

   
}