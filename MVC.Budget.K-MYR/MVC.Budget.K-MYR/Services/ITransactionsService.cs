using MVC.Budget.K_MYR.Models;
using System.Linq.Expressions;

namespace MVC.Budget.K_MYR.Services
{
    public interface ITransactionsService
    {
        Task<List<Transaction>> GetTransactions();
        Transaction? GetByID(int id);
        ValueTask<Transaction?> GetByIdAsync(int id);
        Task<Transaction> AddTransaction(TransactionPost transactionPost);
        Task UpdateTransaction(Transaction transaction, TransactionPut transactionPut);
        Task DeleteTransaction(Transaction transaction);
        Task<bool> CategoryExists(int id);

    }
}