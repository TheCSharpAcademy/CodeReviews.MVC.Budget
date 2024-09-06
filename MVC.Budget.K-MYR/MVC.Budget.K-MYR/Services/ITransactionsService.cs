using MVC.Budget.K_MYR.Models;

namespace MVC.Budget.K_MYR.Services;

public interface ITransactionsService
{
    Task<TransactionsSearchResponse>? GetTransactions(TransactionsSearchRequest searchModel);
    Task<List<Transaction>> GetUnevaluatedTransactions(int categoryid, int? lastId = null, DateTime? lastDate = null, int pageSize = 10);
    Transaction? GetByID(int id);
    ValueTask<Transaction?> GetByIDAsync(int id);
    Task<Transaction> AddTransaction(TransactionPost transactionPost);
    Task UpdateTransaction(Transaction transaction, TransactionPut transactionPut);
    Task DeleteTransaction(Transaction transaction);
}