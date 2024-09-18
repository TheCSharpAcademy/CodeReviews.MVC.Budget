using System.Linq.Expressions;
using Budget.Domain.Entities;

namespace Budget.Application.Repositories;

/// <summary>
/// Defines the contract for accessing and managing Transaction entities in the data store for the Application.
/// </summary>
public interface ITransactionRepository
{
    Task CreateAsync(TransactionEntity entity);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<TransactionEntity>> ReturnAsync(
        Expression<Func<TransactionEntity, bool>>? filter = null,
        Func<IQueryable<TransactionEntity>, IOrderedQueryable<TransactionEntity>>? orderBy = null,
        string includeProperties = "");
    Task<TransactionEntity?> ReturnAsync(object id);
    Task UpdateAsync(TransactionEntity entity);
}
