using MVC.Budget.K_MYR.Data;
using MVC.Budget.K_MYR.Models;

namespace MVC.Budget.K_MYR.Services;

public class TransactionsService : ITransactionsService
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TransactionsService> _logger;

    public TransactionsService(IUnitOfWork unitOfWork, ILogger<TransactionsService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public Task<List<TransactionDTO>> GetTransactions(int? fiscalPlanId, int? categoryId = null, string? searchString = null, DateTime? minDate = null, DateTime? maxDate = null, decimal? minAmount = null, decimal? maxAmount = null)
    {
        return _unitOfWork.TransactionsRepository.GetFilteredTransactionsAsync(fiscalPlanId, categoryId, searchString, minDate, maxDate, minAmount, maxAmount);
    }

    public Task<List<Transaction>> GetUnevaluatedTransactions(int categoryid, int? lastId = null, DateTime? lastDate = null, int pageSize = 10)
    {
        return _unitOfWork.TransactionsRepository.GetUnevaluatedTransactions(categoryid, lastId, lastDate, pageSize);
    }
    public ValueTask<Transaction?> GetByIDAsync(int id)
    {
        return _unitOfWork.TransactionsRepository.GetByIDAsync(id);
    }

    public Transaction? GetByID(int id)
    {
        return _unitOfWork.TransactionsRepository.GetByID(id);
    }

    public async Task<Transaction> AddTransaction(TransactionPost transactionPost)
    {
        var transaction = new Transaction()
        {
            Title = transactionPost.Title,
            DateTime = transactionPost.DateTime,
            Amount = transactionPost.Amount,
            IsHappy = transactionPost.IsHappy,
            IsNecessary = transactionPost.IsNecessary,
            CategoryId = transactionPost.CategoryId,
        };

        _unitOfWork.TransactionsRepository.Insert(transaction);

        await _unitOfWork.Save();

        return transaction;
    }

    public async Task UpdateTransaction(Transaction transaction, TransactionPut transactionPut)
    {
        transaction.Title = transactionPut.Title;
        transaction.Description = transactionPut.Description;
        transaction.Amount = transactionPut.Amount;
        transaction.IsHappy = transactionPut.IsHappy;
        transaction.IsNecessary = transactionPut.IsNecessary;
        transaction.DateTime = transactionPut.DateTime;        
        transaction.Evaluated = transactionPut.Evaluated;
        transaction.PreviousIsNecessary = transactionPut.PreviousIsNecessary;
        transaction.PreviousIsHappy = transactionPut.PreviousIsHappy;

        await _unitOfWork.Save();
    }

    public async Task DeleteTransaction(Transaction transaction)
    {
        _unitOfWork.TransactionsRepository.Delete(transaction);
        await _unitOfWork.Save();
    }
}


