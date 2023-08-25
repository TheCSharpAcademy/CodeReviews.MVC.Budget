using kmakai.Budget.Models;
namespace kmakai.Budget.Repositories;


public interface IRepository
{
    public List<Category> GetCategories();
    public void AddCategory(Category category);
    public void AddTransaction(Transaction transaction);
    public List<Transaction> GetTransactions();
    public List<TransactionType> GetTransactionTypes();
    public void DeleteTransaction(int id);
    public void DeleteCategory(int id);
    public void UpdateTransaction(Transaction transaction);
    public void UpdateCategory(Category category);
}
