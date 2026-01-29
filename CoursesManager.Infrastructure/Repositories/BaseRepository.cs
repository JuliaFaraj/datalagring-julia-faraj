using CoursesManager.Domain.Interfaces;
using CoursesManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CoursesManager.Infrastructure.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly ApplicationDbContext _context;
    private readonly DbSet<TEntity> _table;

    public BaseRepository(ApplicationDbContext context)
    {
        _context = context;
        _table = _context.Set<TEntity>();
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> findBy)
    {
        return await _table.AnyAsync(findBy);
    }

    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        _table.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _table.ToListAsync();
    }

}
