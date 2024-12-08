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
        return _context.Transactions.ToList();
    }

    public TransactionModel? GetTransactionById(int id)
    {
        return _context.Transactions.FirstOrDefault(x=>x.Id == id)??null;
    }
    

    public List<TransactionModel> GetTransactionsByCategoryId(int id)
    {
        return _context.Transactions.Where(t=>t.Category.Id==id).ToList();
    }

    public void AddTransaction(TransactionDto transaction)
    {
        try
        {
            _context.Transactions.Add(new TransactionModel
            {
                Date = transaction.Date,
                Details = transaction.Details,
                Category = _context.Categories.FirstOrDefault(x => x.Id == transaction.CategoryId),
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

    public void UpdateTransaction(int id,TransactionDto transaction)
    {
        try
        {
            _context.Transactions.Update(new TransactionModel{Id = id,Date = transaction.Date,Details = transaction.Details,Category = _context.Categories.FirstOrDefault(x => x.Id == transaction.CategoryId),});
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
            _context.Transactions.Remove(_context.Transactions.FirstOrDefault(x => x.Id == id)!);
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
        return await _context.Transactions.ToListAsync();
    }

    public async Task<TransactionModel?> GetTransactionByIdAsync(int id)
    {
        return await _context.Transactions.FirstOrDefaultAsync(x=>x.Id == id);
    }

   
    

    public async Task AddTransactionAsync(TransactionDto transaction)
    {
        try
        {
            await _context.Transactions.AddAsync(new TransactionModel
            {
                Details = transaction.Details,
                Category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == transaction.CategoryId),
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

    public async Task UpdateTransactionAsync(int id,TransactionDto transaction)
    {
        try
        {
            _context.Transactions.Update(new TransactionModel{Id = id,Date = transaction.Date,Details = transaction.Details,Category = _context.Categories.FirstOrDefault(x => x.Id == transaction.CategoryId),});
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
        try
        {
            var transactionToDelete = await _context.Transactions.FirstOrDefaultAsync(x => x.Id == id);
            _context.Transactions.Remove(transactionToDelete!);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error DeletingAsync transaction : {ex.Message}");
            throw;
        }   
    }
}