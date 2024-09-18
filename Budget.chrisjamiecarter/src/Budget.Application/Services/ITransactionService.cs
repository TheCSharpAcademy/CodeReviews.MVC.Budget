using System.Linq.Expressions;
using Budget.Domain.Entities;

namespace Budget.Application.Services;

/// <summary>
/// Defines the contract for a service that manages Transaction entities.
/// </summary>
public interface ITransactionService
{
    Task CreateAsync(TransactionEntity transaction);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<TransactionEntity>> ReturnAsync(
        Expression<Func<TransactionEntity, bool>>? filter = null,
        Func<IQueryable<TransactionEntity>, IOrderedQueryable<TransactionEntity>>? orderBy = null,
        string includeProperties = "");
    Task<TransactionEntity?> ReturnAsync(Guid id);
    Task UpdateAsync(TransactionEntity transaction);
}