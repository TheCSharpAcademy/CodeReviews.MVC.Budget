using Microsoft.EntityFrameworkCore;
using MVC.Budget.K_MYR.Data;
using MVC.Budget.K_MYR.Models;
using System.Linq.Expressions;


namespace MVC.Budget.K_MYR.Repositories;

public sealed class CategoriesRepository : GenericRepository<Category>, ICategoriesRepository
{
    public CategoriesRepository(DatabaseContext context) : base(context)
    { }

    public Task<Category?> GetCategoryAsync(int id)
    {
        return _dbSet
                .Include(c => c.Transactions.OrderByDescending(t => t.DateTime))
                .SingleOrDefaultAsync(c => c.Id == id);
    }

    public Task<Category?> GetCategoryWithFilteredStatistics(int id, Expression<Func<Category, IEnumerable<CategoryBudget>>> filter)
    {
        return _dbSet.Include(filter)
                     .SingleOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<CategoryStatistics>> GetMonthlyStatistics(int groupId, int year)
    {
        var query = _dbSet.Where(c => c.GroupId == groupId)
                     .Include(c => c.Transactions.Where(t => t.DateTime.Year == year))
                     .Include(c => c.PreviousBudgets.Where(t => t.Month.Year == year))
                     .Select(c => new CategoryStatistics
                     {
                         Category = c.Name,
                         Budget = c.Budget,
                         Statistics = c.Transactions.GroupBy(t => t.DateTime.Month)
                                               .Select(g => new MonthlyStatistics
                                               {
                                                   Month = g.Key,
                                                   TotalSpent = g.Sum(t => t.Amount),
                                                   HappyTransactions = g.Where(t => t.PreviousIsHappy && t.Evaluated || !t.Evaluated && t.IsHappy).Sum(t => t.Amount),
                                                   UnhappyTransactions = g.Where(t => !t.PreviousIsHappy && t.Evaluated || !t.Evaluated && !t.IsHappy).Sum(t => t.Amount),
                                                   HappyEvaluatedTransactions = g.Where(t => t.IsHappy && t.Evaluated).Sum(t => t.Amount),
                                                   UnhappyEvaluatedTransactions = g.Where(t => !t.IsHappy && t.Evaluated).Sum(t => t.Amount),
                                                   NecessaryTransactions = g.Where(t => t.PreviousIsNecessary && t.Evaluated || !t.Evaluated && t.IsNecessary).Sum(t => t.Amount),
                                                   UnnecessaryTransactions = g.Where(t => !t.PreviousIsNecessary && t.Evaluated || !t.Evaluated && !t.IsNecessary).Sum(t => t.Amount),
                                                   NecessaryEvaluatedTransactions = g.Where(t => t.IsNecessary && t.Evaluated).Sum(t => t.Amount),
                                                   UnnecessaryEvaluatedTransactions = g.Where(t => !t.IsNecessary && t.Evaluated).Sum(t => t.Amount),
                                                   UnevaluatedTransactions = g.Where(t => !t.Evaluated).Sum(t => t.Amount),
                                               }),
                         BudgetLimits = c.PreviousBudgets.Select(s => new BudgetLimit { Budget = s.Budget, Month = s.Month })
                     });

        return await query.AsNoTracking().ToListAsync();
    }

    public Task<List<Category>> GetCategoriesWithFilteredTransactionsAsync(Expression<Func<Category, bool>>? filter = null, Func<IQueryable<Category>,
                                                IOrderedQueryable<Category>>? orderBy = null,
                                                Expression<Func<Category,
                                                IOrderedEnumerable<Transaction>>>? filterTransactions = null)
    {
        IQueryable<Category> query = _dbSet;

        if (filter is not null)
            query = query.Where(filter);

        if (filterTransactions is not null)
        {
            query = query.Include(filterTransactions);
        }

        if (orderBy is not null)
            return orderBy(query).ToListAsync();

        return query.ToListAsync();
    }
}
