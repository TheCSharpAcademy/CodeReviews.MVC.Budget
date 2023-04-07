using System.Linq.Expressions;
using Budget.Context;
using Budget.Models;
using Microsoft.EntityFrameworkCore;

namespace Budget.Repositories;

public class TransactionRepository : Repository<Transaction>, ITransactionRepository
{
    public TransactionRepository(BudgetDbContext budgetDbContext) : base(budgetDbContext)
    {
    }

    public async Task<IEnumerable<Transaction>?> GetTransactionsWithCategories(Expression<Func<Transaction, bool>> predicate)
    {
        var transactionsWithCategories = DbEntitySet
            .Where(predicate)
            .Include(tr => tr.Category);

        return await transactionsWithCategories.ToListAsync();
    }

    public override async Task<Transaction?> GetEntity(int? id)
    {
        var entity = await DbEntitySet.AsNoTracking().FirstOrDefaultAsync(tr => tr.Id == id);

        return entity;
    }
}