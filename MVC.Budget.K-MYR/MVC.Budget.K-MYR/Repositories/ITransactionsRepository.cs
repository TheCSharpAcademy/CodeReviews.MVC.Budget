using MVC.Budget.K_MYR.Models;

namespace MVC.Budget.K_MYR.Repositories;

public interface ITransactionsRepository : IGenericRepository<Transaction>
{
    Task<List<TransactionDTO>> GetFilteredTransactionsAsync(int? fiscalPlanId, int? categoryId, string? searchString, DateTime? minDate, DateTime? maxDate, decimal? minAmount, decimal? maxAmount);
    Task<List<Transaction>> GetUnevaluatedTransactions(int categoryid, int? lastId = null, DateTime? lastDate = null, int pageSize = 10);
}