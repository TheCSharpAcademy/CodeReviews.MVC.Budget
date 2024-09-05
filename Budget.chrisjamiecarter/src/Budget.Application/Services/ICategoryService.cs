using System.Linq.Expressions;
using Budget.Domain.Entities;

namespace Budget.Application.Services;

/// <summary>
/// Defines the contract for a service that manages Category entities.
/// </summary>
public interface ICategoryService
{
    Task CreateAsync(CategoryEntity category);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<CategoryEntity>> ReturnAsync(
        Expression<Func<CategoryEntity, bool>>? filter = null,
        Func<IQueryable<CategoryEntity>, IOrderedQueryable<CategoryEntity>>? orderBy = null,
        string includeProperties = "");
    Task<CategoryEntity?> ReturnAsync(Guid id);
    Task UpdateAsync(CategoryEntity category);
}