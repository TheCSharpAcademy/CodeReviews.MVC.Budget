using Budget.Doc415.Models;

namespace Budget.Doc415.Repositories
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetAllTransactions(string SearchName, DateTime StartDate, DateTime EndDate, int? categoryId);
        Task<Transaction> GetTransactionById(int id);
        Task DeleteTransaction(int id);
        Task InsertTransaction(Transaction transaction);
        Task UpdateTransaction(int id, Transaction transaction);
        bool IsSeeded();
    }
}
