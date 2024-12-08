using Budget.hasona23.Models;

namespace Budget.hasona23.Services;

public interface ITransactionService
{
    List<TransactionModel> GetAllTransactions();
    TransactionModel? GetTransactionById(int id);
    void AddTransaction(TransactionDto transaction);
    void UpdateTransaction(int id,TransactionDto transaction);
    void DeleteTransaction(int id);
    //List<TransactionModel> GetTransactionsByCategoryId(int id);
    Task<List<TransactionModel>> GetAllTransactionsAsync();
    Task<TransactionModel?> GetTransactionByIdAsync(int id);
    //Task<List<TransactionModel>> GetTransactionsByCategoryIdAsync(int id);
    Task AddTransactionAsync(TransactionDto transaction);
    Task UpdateTransactionAsync(int id,TransactionDto transaction);
    Task DeleteTransactionAsync(int id);
}