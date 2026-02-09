using System.Linq.Expressions;
using TestTask.Core.Interfaces;

namespace TestTask.Core.Repositories;

public interface IGenericRepository<TEntity, TId> where TEntity : class, IIdentifiablyEntity<TId>
{
    void Create(TEntity entity);

    void CreateRange(IEnumerable<TEntity> entities);

    void Update(TEntity entity, params Expression<Func<TEntity, object?>>[] updatedProperties);

    void Delete(TEntity entity);

    Task DeleteAsync(TId id);

    Task<TEntity?> GetByIdAsync(
        TId id,
        string? includeMany = null,
        Expression<Func<TEntity, object>>? include = null,
        bool withoutTracking = true,
        bool ignoreGlobalFilter = false,
        bool useSplitQuery = false);

    Task<TEntity?> GetSingleAsync(
        Expression<Func<TEntity, bool>> filter,
        string? includeMany = null,
        Expression<Func<TEntity, object>>? include = null,
        bool withoutTracking = true,
        bool ignoreGlobalFilter = false,
        bool useSplitQuery = false);

    Task<IReadOnlyList<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string? includeMany = null,
        Expression<Func<TEntity, object>>? include = null,
        int? skip = null,
        int? take = null,
        bool withoutTracking = true,
        bool ignoreGlobalFilter = false,
        bool useSplitQuery = false);
}
