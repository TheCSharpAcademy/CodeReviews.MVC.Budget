using Budget.hasona23.Data;
using Budget.hasona23.Models;
using Microsoft.EntityFrameworkCore;

namespace Budget.hasona23.Services;

public class TransactionService : ITransactionService
{
    private BudgetContext _context;

    public TransactionService(BudgetContext context)
    {
        _context = context;
    }

    public List<TransactionModel> GetAllTransactions()
    {
        return _context.Transactions.Include(t => t.Category).ToList();
    }

    public TransactionModel? GetTransactionById(int id)
    {
        return _context.Transactions.FirstOrDefault(x => x.Id == id) ?? null;
    }


    public List<TransactionModel> GetTransactionsByCategoryId(int id)
    {
        return _context.Transactions.Include(t => t.Category).Where(t => t.Category.Id == id).ToList();
    }

    public void AddTransaction(TransactionDto transaction)
    {
        CategoryModel? category = _context.Categories.FirstOrDefault(x => x.Id == transaction.CategoryId);
        if (category == null)
        {
            throw new ArgumentException("Category doesn't exist");
        }

        try
        {
            _context.Transactions.Add(new TransactionModel
            {
                Date = transaction.Date,
                Details = transaction.Details,
                Category = category,
                Price = transaction.Price,
            });
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding transaction {transaction.Details}: {ex.Message}");
            throw;
        }
    }

    public void UpdateTransaction(int id, TransactionDto transaction)
    {
        CategoryModel? category = _context.Categories.FirstOrDefault(x => x.Id == id);
        if (category == null)
        {
            throw new ArgumentException("Category doesn't exist");
        }

        try
        {
            _context.Transactions.Update(new TransactionModel
            {
                Id = id, Date = transaction.Date, Details = transaction.Details, Category = category,
                Price = transaction.Price,
            });
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating transaction {transaction.Details}: {ex.Message}");
            throw;
        }
    }

    public void DeleteTransaction(int id)
    {
        try
        {
            _context.Transactions.Where(t => t.Id == id).ExecuteDelete();
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error Deleting transaction : {ex.Message}");
            throw;
        }
    }

    public async Task<List<TransactionModel>> GetAllTransactionsAsync()
    {
        return await _context.Transactions.Include(t => t.Category).ToListAsync();
    }

    public async Task<TransactionModel?> GetTransactionByIdAsync(int id)
    {
        return await _context.Transactions.Include(t => t.Category).FirstOrDefaultAsync(x => x.Id == id);
    }


    public async Task AddTransactionAsync(TransactionDto transaction)
    {
        CategoryModel? category = _context.Categories.FirstOrDefault(x => x.Id == transaction.CategoryId);
        if (category == null)
        {
            throw new ArgumentException("Category doesn't exist");
        }

        try
        {
            await _context.Transactions.AddAsync(new TransactionModel
            {
                Details = transaction.Details,
                Category = category,
                Price = transaction.Price,
                Date = transaction.Date,
            });
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error addingAsync transaction {transaction.Details}: {ex.Message}");
            throw;
        }
    }

    public async Task UpdateTransactionAsync(int id, TransactionDto transaction)
    {
        CategoryModel? category = _context.Categories.FirstOrDefault(x => x.Id == transaction.CategoryId);
        if (category == null)
        {
            throw new ArgumentException("Category doesn't exist");
        }

        try
        {
            var result = _context.Transactions.SingleOrDefault(c => c.Id == id);
            if (result != null)
            {
                result.Details = transaction.Details;
                result.Date = transaction.Date;
                result.Price = transaction.Price;
                result.Category = category;
                await _context.SaveChangesAsync();
            }
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updatingAsync transaction {transaction.Details}: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteTransactionAsync(int id)
    {
        CategoryModel? category = _context.Categories.FirstOrDefault(x => x.Id == id);
        if (category == null)
        {
            throw new ArgumentException("Category doesn't exist");
        }

        try
        {
            await _context.Transactions.Where(t => t.Id == id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error DeletingAsync transaction : {ex.Message}");
            throw;
        }
    }
}