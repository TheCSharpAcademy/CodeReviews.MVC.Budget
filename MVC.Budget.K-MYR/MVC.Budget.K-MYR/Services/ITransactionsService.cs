using MVC.Budget.K_MYR.Models;
using System.Linq.Expressions;

namespace MVC.Budget.K_MYR.Services
{
    public interface ITransactionsService
    {
        Task<Transaction> AddTransaction(TransactionPost transactionPost, Category category);
        Task DeleteTransaction(Transaction transaction);
        Transaction? GetByID(int id);
        ValueTask<Transaction?> GetByIDAsync(int id);
        Task<Category?> GetCategoryWithFilteredStatistics(int id, Expression<Func<Category, IEnumerable<CategoryStatistic>>> filter);
        Task<List<Transaction>> GetTransactions();
        Task UpdateTransaction(Category category, Transaction transaction, TransactionPut transactionPut);
    }
}