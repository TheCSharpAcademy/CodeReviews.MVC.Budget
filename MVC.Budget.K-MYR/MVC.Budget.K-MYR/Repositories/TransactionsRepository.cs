using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MVC.Budget.K_MYR.Data;
using MVC.Budget.K_MYR.Models;

namespace MVC.Budget.K_MYR.Repositories;

public sealed class TransactionsRepository : GenericRepository<Transaction>, ITransactionsRepository
{
    public TransactionsRepository(DatabaseContext context) : base(context)
    { }

    public async Task<List<TransactionDTO>> GetFilteredTransactionsAsync(int? fiscalPlanId, int? categoryId = null, string? searchString = null, DateTime? minDate = null, DateTime? maxDate = null, decimal? minAmount = null, decimal? maxAmount = null)
    {
        IQueryable<Transaction> query = _dbSet;

        if (fiscalPlanId is not null)
            query = query.Where(t => t.Category.FiscalPlanId == fiscalPlanId);

        if (categoryId is not null)
            query = query.Where(t => t.CategoryId == categoryId);

        if (minDate is not null)
            query = query.Where(t => t.DateTime >= minDate);

        if (maxDate is not null)
            query = query.Where(t => t.DateTime <= maxDate);

        if (minAmount is not null)
            query = query.Where(t => t.Amount >= minAmount);

        if (maxAmount is not null)
            query = query.Where(t => t.Amount <= maxAmount);

        if (!searchString.IsNullOrEmpty())
            query = query.Where(t => t.Title.Contains(searchString!.Trim()));

        return await query.Select(t => new TransactionDTO
                           {
                               Id = t.Id,
                               Title = t.Title,
                               Amount = t.Amount,
                               Description = t.Description,
                               DateTime = t.DateTime,
                               Category = t.Category.Name
                           })
                           .ToListAsync();
    }
}
