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

    public Task<CategoryMonthDTO?> GetCategoryDataByMonth(int id, DateTime month)
    {
        DateTime thisMonth = new(month.Year, month.Month, 1);
        var nextMonth = thisMonth.AddMonths(1);
        DateTime cutOffDate = new(nextMonth.Year, nextMonth.Month, 1);
        var query = _dbSet
                      .Where(c => c.Id == id)
                      .Select(c => new CategoryMonthDTO
                      {
                          Id = c.Id,
                          FiscalPlanId = c.FiscalPlanId,
                          CategoryType = c.CategoryType,
                          Name = c.Name,
                          Budget = c.Budget,
                          BudgetLimit = c.PreviousBudgets.Where(b => b.Month < cutOffDate)
                                                         .Select(b => new BudgetLimit
                                                         {
                                                             Budget = b.Budget,
                                                             Month = b.Month
                                                         })
                                                         .OrderByDescending(b => b.Month)
                                                         .FirstOrDefault(),
                          Total = c.Transactions.Where(t => t.DateTime >= thisMonth && t.DateTime < nextMonth)
                                                .Sum(t => t.Amount),
                          HappyTotal = c.Transactions.Where(t => t.DateTime >= thisMonth && t.DateTime < nextMonth)
                                                     .Where(t => t.PreviousIsHappy && t.IsEvaluated || !t.IsEvaluated && t.IsHappy)
                                                     .Sum(t => t.Amount),
                          NecessaryTotal = c.Transactions.Where(t => t.DateTime >= thisMonth && t.DateTime < nextMonth)
                                                         .Where(t => t.PreviousIsNecessary && t.IsEvaluated || !t.IsEvaluated && t.IsNecessary)
                                                         .Sum(t => t.Amount),
                      });

        return query.AsNoTracking()
                    .AsSplitQuery()
                    .FirstOrDefaultAsync();
    }


    public Task<List<CategoryDTO>> GetCategoriesDataByMonth(int fiscalPlanId, DateTime month)
    {
        DateTime thisMonth = new(month.Year, month.Month, 1);
        var nextMonth = thisMonth.AddMonths(1);
        DateTime cutOffDate = new(nextMonth.Year, nextMonth.Month, 1);
        var query = _dbSet
                      .Where(c => c.FiscalPlanId == fiscalPlanId)
                      .Select(c => new CategoryDTO
                      {
                          Id = c.Id,
                          CategoryType = c.CategoryType,
                          Name = c.Name,
                          Budget = c.Budget,
                          BudgetLimit = c.PreviousBudgets.Where(b => b.Month < cutOffDate)
                                                         .Select(b => new BudgetLimit
                                                         {
                                                             Budget = b.Budget,
                                                             Month = b.Month
                                                         })
                                                         .OrderByDescending(b => b.Month)
                                                         .FirstOrDefault(),
                          Total = c.Transactions.Where(t => t.DateTime >= thisMonth && t.DateTime < nextMonth)
                                                .Sum(t => t.Amount),
                          HappyTotal = c.Transactions.Where(t => t.DateTime >= thisMonth && t.DateTime < nextMonth)
                                                     .Where(t => t.PreviousIsHappy && t.IsEvaluated || !t.IsEvaluated && t.IsHappy)
                                                     .Sum(t => t.Amount),
                          NecessaryTotal = c.Transactions.Where(t => t.DateTime >= thisMonth && t.DateTime < nextMonth)
                                                         .Where(t => t.PreviousIsNecessary && t.IsEvaluated || !t.IsEvaluated && t.IsNecessary)
                                                         .Sum(t => t.Amount),
                      });

        return query.AsNoTracking()
                    .AsSplitQuery()
                    .ToListAsync();
    }

    public Task<List<CategoryStatisticsDTO>> GetCategoriesDataByYear(int id, int year)
    {
        DateTime thisYear = new(year, 1, 1);
        DateTime nextYear = new(year + 1, 1, 1);

        var query = _dbSet.Where(c => c.FiscalPlanId == id && c.CategoryType == 2)
                          .Select(c => new CategoryStatisticsDTO
                          {
                              Category = c.Name,
                              CategoryType = c.CategoryType,
                              Budget = c.Budget,
                              Statistics = c.Transactions.Where(t => t.DateTime >= thisYear && t.DateTime < nextYear)
                                                         .GroupBy(t => t.DateTime.Month)
                                                         .Select(g => new MonthlyStatistic
                                                         {
                                                             Month = g.Key,
                                                             TotalSpent = g.Sum(t => t.Amount),
                                                             HappyTransactions = g.Where(t => t.PreviousIsHappy && t.IsEvaluated || !t.IsEvaluated && t.IsHappy)
                                                                                  .Sum(t => t.Amount),
                                                             UnhappyTransactions = g.Where(t => !t.PreviousIsHappy && t.IsEvaluated || !t.IsEvaluated && !t.IsHappy)
                                                                                    .Sum(t => t.Amount),
                                                             HappyEvaluatedTransactions = g.Where(t => t.IsHappy && t.IsEvaluated)
                                                                                           .Sum(t => t.Amount),
                                                             UnhappyEvaluatedTransactions = g.Where(t => !t.IsHappy && t.IsEvaluated)
                                                                                             .Sum(t => t.Amount),
                                                             NecessaryTransactions = g.Where(t => t.PreviousIsNecessary && t.IsEvaluated || !t.IsEvaluated && t.IsNecessary)
                                                                                      .Sum(t => t.Amount),
                                                             UnnecessaryTransactions = g.Where(t => !t.PreviousIsNecessary && t.IsEvaluated || !t.IsEvaluated && !t.IsNecessary)
                                                                                        .Sum(t => t.Amount),
                                                             NecessaryEvaluatedTransactions = g.Where(t => t.IsNecessary && t.IsEvaluated)
                                                                                               .Sum(t => t.Amount),
                                                             UnnecessaryEvaluatedTransactions = g.Where(t => !t.IsNecessary && t.IsEvaluated)
                                                                                                 .Sum(t => t.Amount),
                                                             UnevaluatedTransactions = g.Where(t => !t.IsEvaluated)
                                                                                        .Sum(t => t.Amount),
                                                         }),
                              BudgetLimits = c.PreviousBudgets.Where(t => t.Month.Year <= year)
                                                              .OrderBy(bl => bl.Month)
                                                              .Take(12)
                                                              .Select(s => new BudgetLimit { Budget = s.Budget, Month = s.Month })
                          });

        return query.AsNoTracking()
                    .AsSplitQuery()
                    .ToListAsync();
    }

    public Task<Category?> GetCategoryWithBudgetLimits(int id, Expression<Func<Category, IEnumerable<CategoryBudget>>> filter)
    {
        return _dbSet.Include(filter)
                     .SingleOrDefaultAsync(c => c.Id == id);
    }

    public Task<List<Category>> GetCategoriesWithFilteredTransactionsAsync(Expression<Func<Category, bool>>? filter = null,
        Func<IQueryable<Category>, IOrderedQueryable<Category>>? orderBy = null,
        Expression<Func<Category, IEnumerable<Transaction>>>? filterTransactions = null)
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
