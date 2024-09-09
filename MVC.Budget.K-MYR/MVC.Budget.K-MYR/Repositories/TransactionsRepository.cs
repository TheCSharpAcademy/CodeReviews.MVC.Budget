using Microsoft.EntityFrameworkCore;
using MVC.Budget.K_MYR.Data;
using MVC.Budget.K_MYR.Models;

namespace MVC.Budget.K_MYR.Repositories;

public sealed class TransactionsRepository : GenericRepository<Transaction>, ITransactionsRepository
{
    public TransactionsRepository(DatabaseContext context) : base(context)
    { }

    public Task<List<TransactionDTO>> GetAsync(TransactionsSearch searchModel, Func<IQueryable<Transaction>,
                                                 IOrderedQueryable<Transaction>> orderBy, Func<IQueryable<Transaction>,
                                                 IQueryable<Transaction>>? filter = null)
    {
        IQueryable<Transaction> query = _dbSet;

        if (filter is not null)
            query = filter(query);

        if (searchModel.FiscalPlanId is not null)
            query = query.Where(t => t.Category.FiscalPlanId == searchModel.FiscalPlanId);

        if (searchModel.CategoryId is not null)
            query = query.Where(t => t.CategoryId == searchModel.CategoryId);

        if (searchModel.MinDate is not null)
            query = query.Where(t => t.DateTime >= searchModel.MinDate);

        if (searchModel.MaxDate is not null)
            query = query.Where(t => t.DateTime <= searchModel.MaxDate);

        if (searchModel.MinAmount is not null)
            query = query.Where(t => t.Amount >= searchModel.MinAmount);

        if (searchModel.MaxAmount is not null)
            query = query.Where(t => t.Amount <= searchModel.MaxAmount);

        if (!String.IsNullOrEmpty(searchModel.SearchString))
            query = query.Where(t => t.Title.Contains(searchModel.SearchString.Trim()));

        if (orderBy is not null)
            query = orderBy(query);

        return query.Select(t => new TransactionDTO
        {
            Id = t.Id,
            Title = t.Title,
            Amount = t.Amount,
            Description = t.Description,
            DateTime = t.DateTime,
            Category = t.Category.Name
        })
                    .Take(searchModel.PageSize)
                    .AsNoTracking()
                    .ToListAsync();
    }

    public Task<List<Transaction>> GetUnevaluatedTransactions(int categoryid, int? lastId = null, DateTime? lastDate = null, int pageSize = 10)
    {
        var query = _dbSet.Where(t => t.CategoryId == categoryid);

        if (lastId is not null && lastDate is not null)
        {
            query = query.Where(t => t.DateTime > lastDate || (t.DateTime == lastDate && t.Id > lastId));
        }

        return query.OrderBy(t => t.DateTime)
                     .ThenBy(t => t.Id)
                     .Take(pageSize)
                     .AsNoTracking()
                     .ToListAsync();
    }
}
