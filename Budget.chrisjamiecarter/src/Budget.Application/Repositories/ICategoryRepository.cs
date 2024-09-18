using System.Linq.Expressions;
using Budget.Domain.Entities;

namespace Budget.Application.Repositories;

/// <summary>
/// Defines the contract for accessing and managing Category entities in the data store for the Application.
/// </summary>
public interface ICategoryRepository
{
    Task CreateAsync(CategoryEntity entity);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<CategoryEntity>> ReturnAsync(
        Expression<Func<CategoryEntity, bool>>? filter = null,
        Func<IQueryable<CategoryEntity>, IOrderedQueryable<CategoryEntity>>? orderBy = null,
        string includeProperties = "");
    Task<CategoryEntity?> ReturnAsync(object id);
    Task UpdateAsync(CategoryEntity entity);
}
