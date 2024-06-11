using Budget.Doc415.Data;
using Budget.Doc415.Models;
using Microsoft.EntityFrameworkCore;

namespace Budget.Doc415.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly IDbContextFactory<BudgetDb> _fcontext;

    public TransactionRepository(IDbContextFactory<BudgetDb> fcontext)
    {
        _fcontext = fcontext;
    }

    public async Task<List<Transaction>> GetAllTransactions(string SearchName, DateTime StartDate, DateTime EndDate, int? categoryId)
    {
        using var _context = _fcontext.CreateDbContext();
        IQueryable<string> transactionQuery = from m in _context.Transactions
                                              orderby m.Name
                                              select m.Name;
        var transactions = from m in _context.Transactions
                           select m;
        try
        {

            if (!string.IsNullOrEmpty(SearchName))
            {
                transactions = transactions.Where(s => s.Name == SearchName);
            }

            if (categoryId != null)
            {
                transactions = transactions.Where(x => x.RefCatId == categoryId);
            }

            if (StartDate != null && EndDate != DateTime.MinValue)
            {
                transactions = transactions.Where(x => x.Date > StartDate);
            }

            if (EndDate != null && EndDate != DateTime.MinValue)
            {
                transactions = transactions.Where(x => x.Date < EndDate);
            }

            return await transactions.ToListAsync();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Database exception: {ex}");
            return await Task.FromResult(new List<Transaction>());
        }
    }

    public async Task<Transaction> GetTransactionById(int id)
    {
        using var _context = _fcontext.CreateDbContext();
        try
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(x => x.Id == id);
            return transaction;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Database exception: {ex}");
            return await Task.FromResult(new Transaction());
        }
    }

    public async Task DeleteTransaction(int id)
    {
        using var _context = _fcontext.CreateDbContext();
        try
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(x => x.Id == id);

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Database exception: {ex}");
        }
    }

    public async Task InsertTransaction(Transaction transaction)
    {
        using var _context = _fcontext.CreateDbContext();
        try
        {
            await _context.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Database exception: {ex}");
        }
    }

    public async Task UpdateTransaction(int id, Transaction transaction)
    {
        using var _context = _fcontext.CreateDbContext();
        try
        {
            var toUpdate = await _context.Transactions.FirstOrDefaultAsync(x => x.Id == id);
            toUpdate.Name = transaction.Name;
            toUpdate.Date = transaction.Date;
            toUpdate.Description = transaction.Description;
            toUpdate.Amount = transaction.Amount;
            toUpdate.RefCatId = transaction.RefCatId;

            _context.Update(toUpdate);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Database exception: {ex}");
        }
    }

    public bool IsSeeded()
    {
        using var _context = _fcontext.CreateDbContext();
        return _context.Transactions.Any();
    }
}
