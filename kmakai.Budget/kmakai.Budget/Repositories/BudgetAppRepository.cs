using kmakai.Budget.Context;
using kmakai.Budget.Models;

namespace kmakai.Budget.Repositories;

public class BudgetAppRepository: IRepository
{
    private readonly BudgetContext _context;

    public BudgetAppRepository(BudgetContext context)
    {
        _context = context;
    }

    public List<Category> GetCategories()
    {
        return _context.Categories.ToList();
    }

    public void AddCategory(Category category)
    {
        if (category != null)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }
    }

    public void AddTransaction(Transaction transaction)
    {
        if (transaction != null)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
        }
    }

    public List<Transaction> GetTransactions()
    {
        return _context.Transactions.ToList();
    }

    public List<TransactionType> GetTransactionTypes()
    {
        return _context.TransactionTypes.ToList();
    }

    public void DeleteTransaction(int id)
    {
        var transaction = _context.Transactions.Find(id);
        if (transaction != null)
        {
            _context.Transactions.Remove(transaction);
            _context.SaveChanges();
        }
    }

    public void DeleteCategory(int id)
    {
        var category = _context.Categories.Find(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }
    }

    public void UpdateTransaction(Transaction transaction)
    {
        if (transaction != null)
        {
            _context.Transactions.Update(transaction);
            _context.SaveChanges();
        }
    }

    public void UpdateCategory(Category category)
    {
        if (category != null)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
        }
    }

    
}
