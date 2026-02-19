using System.Linq.Expressions;

namespace CoursesManager.Application.Abstractions.Persistence
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        // CREATE
        Task<TEntity> CreateAsync(TEntity entity, CancellationToken ct = default);

        // READ
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> findBy, CancellationToken ct = default);

        Task<TEntity?> GetOneAsync(
            Expression<Func<TEntity, bool>> where,
            bool tracking = false,
            CancellationToken ct = default,
            params Expression<Func<TEntity, object>>[] includes
        );

        Task<IReadOnlyList<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>>? where = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            bool tracking = false,
            CancellationToken ct = default,
            params Expression<Func<TEntity, object>>[] includes
        );

        Task<IReadOnlyList<TSelect>> GetAllAsync<TSelect>(
            Expression<Func<TEntity, TSelect>> select,
            Expression<Func<TEntity, bool>>? where = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            bool tracking = false,
            CancellationToken ct = default,
            params Expression<Func<TEntity, object>>[] includes
        );

        // UPDATE
        void Update(TEntity entity);

        // DELETE
        void Remove(TEntity entity);

        // UNIT OF WORK
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
