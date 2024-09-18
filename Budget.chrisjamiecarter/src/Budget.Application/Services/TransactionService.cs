using System.Linq.Expressions;
using Budget.Application.Repositories;
using Budget.Domain.Entities;

namespace Budget.Application.Services;

/// <summary>
/// Service class responsible for managing operations related to the Transaction entity.
/// Provides methods for creating, updating, deleting, and retrieving category data 
/// by interacting with the underlying data repositories through the Unit of Work pattern.
/// </summary>
public class TransactionService : ITransactionService
{
    #region Fields

    private readonly IUnitOfWork _unitOfWork;

    #endregion
    #region Constructors

    public TransactionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    #endregion
    #region Methods

    public async Task CreateAsync(TransactionEntity transaction)
    {
        await _unitOfWork.Transactions.CreateAsync(transaction);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _unitOfWork.Transactions.DeleteAsync(id);
        await _unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<TransactionEntity>> ReturnAsync(
        Expression<Func<TransactionEntity, bool>>? filter = null,
        Func<IQueryable<TransactionEntity>, IOrderedQueryable<TransactionEntity>>? orderBy = null,
        string includeProperties = "")
    {
        return await _unitOfWork.Transactions.ReturnAsync(filter, orderBy, includeProperties);
    }

    public async Task<TransactionEntity?> ReturnAsync(Guid id)
    {
        return await _unitOfWork.Transactions.ReturnAsync(id);
    }

    public async Task UpdateAsync(TransactionEntity transaction)
    {
        await _unitOfWork.Transactions.UpdateAsync(transaction);
        await _unitOfWork.SaveAsync();
    }

    #endregion
}
