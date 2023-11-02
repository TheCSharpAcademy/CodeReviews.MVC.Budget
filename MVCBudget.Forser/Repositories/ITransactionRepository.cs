namespace MVCBudget.Forser.Repositories
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        Task<Transaction> GetTransactionById(int? id);
        bool DeleteTransaction(Transaction transaction);
        Task<List<Transaction>> GetAllTransactionsAsync();
        List<Transaction> GetAllTransactions();
    }
}