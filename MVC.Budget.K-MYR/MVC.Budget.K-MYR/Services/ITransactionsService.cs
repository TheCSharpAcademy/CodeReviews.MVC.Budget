using MVC.Budget.K_MYR.Models;

namespace MVC.Budget.K_MYR.Services;

public interface ITransactionsService
{
    Task<List<TransactionDTO>> GetTransactions(int? fiscalPlanId, int? categoryId = null, string? searchString = null, DateTime? minDate = null, DateTime? maxDate = null, decimal? minAmount = null, decimal? maxAmount = null);
    Transaction? GetByID(int id);
    ValueTask<Transaction?> GetByIDAsync(int id);
    Task<Transaction> AddTransaction(TransactionPost transactionPost);
    Task UpdateTransaction(Transaction transaction, TransactionPut transactionPut);
    Task DeleteTransaction(Transaction transaction);
}