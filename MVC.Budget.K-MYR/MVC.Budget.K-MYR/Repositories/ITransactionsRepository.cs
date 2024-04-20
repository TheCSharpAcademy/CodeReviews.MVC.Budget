using MVC.Budget.K_MYR.Models;

namespace MVC.Budget.K_MYR.Repositories;

public interface ITransactionsRepository : IGenericRepository<Transaction>
{
    Task<List<TransactionDTO>> GetFilteredTransactionsAsync(int? categoryId, string? searchString, DateTime? minDate, DateTime? maxDate, decimal? minAmount, decimal? maxAmount);
}