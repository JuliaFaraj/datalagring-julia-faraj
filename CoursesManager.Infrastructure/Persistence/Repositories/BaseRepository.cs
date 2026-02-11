using CoursesManager.Application.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CoursesManager.Infrastructure.Persistence.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly AppDbContext _context;
    private readonly DbSet<TEntity> _table;

    public BaseRepository(AppDbContext context)
    {
        _context = context;
        _table = _context.Set<TEntity>();
    }

    // CREATE
    public virtual async Task<TEntity> CreateAsync(TEntity entity, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        _table.Add(entity);
        await _context.SaveChangesAsync(ct);
        return entity;
    }

    // READ
    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> findBy, CancellationToken ct = default)
    {
        return await _table.AnyAsync(findBy, ct);
    }

    public virtual async Task<TEntity?> GetOneAsync(
        Expression<Func<TEntity, bool>> where,
        bool tracking = false,
        CancellationToken ct = default,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var query = BuildQuery(tracking, includes);
        return await query.FirstOrDefaultAsync(where, ct);
    }

    public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool tracking = false,
        CancellationToken ct = default,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var query = BuildQuery(tracking, includes);

        if (where is not null)
            query = query.Where(where);

        if (orderBy is not null)
            query = orderBy(query);

        return await query.ToListAsync(ct);
    }

    public virtual async Task<IReadOnlyList<TSelect>> GetAllAsync<TSelect>(
        Expression<Func<TEntity, TSelect>> select,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool tracking = false,
        CancellationToken ct = default,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var query = BuildQuery(tracking, includes);

        if (where is not null)
            query = query.Where(where);

        if (orderBy is not null)
            query = orderBy(query);

        return await query.Select(select).ToListAsync(ct);
    }

    // UPDATE
    public virtual void Update(TEntity entity)
    {
        _table.Update(entity);
    }

    // DELETE
    public virtual void Remove(TEntity entity)
    {
        _table.Remove(entity);
    }

    // UNIT OF WORK
    public virtual async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }

    private IQueryable<TEntity> BuildQuery(bool tracking, params Expression<Func<TEntity, object>>[] includes)
    {
        var query = tracking ? _table.AsTracking() : _table.AsNoTracking();

        foreach (var include in includes)
            query = query.Include(include);

        return query;
    }
}
