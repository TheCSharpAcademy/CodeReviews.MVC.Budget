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

    public async Task<FiscalPlanDTO?> GetDataByMonth(int id, DateTime month)
    {
        var cutOffDate = month.AddMonths(1);
        cutOffDate = new DateTime(cutOffDate.Year, cutOffDate.Month, 1);
        var query = _dbSet
                      .Where(c => c.FiscalPlanId == id)
                      .Include(c => c.Transactions)
                      .Include(c => c.PreviousBudgets)
                      .GroupBy(c => 1)
                      .Select(g => new FiscalPlanDTO
                      {
                          Id = id,
                          IncomeCategories = g.Where(c => c.CategoryType == 1)
                                              .Select(c => new CategoryDTO
                                              {
                                                  Id = c.Id,
                                                  CategoryType = c.CategoryType,
                                                  Name = c.Name,
                                                  Budget = c.Budget, 
                                                  BudgetLimit = c.PreviousBudgets
                                                                    .Select(b => new BudgetLimit
                                                                    {
                                                                        Budget = b.Budget,
                                                                        Month = b.Month   
                                                                    })
                                                                    .OrderBy(b => b.Month >= cutOffDate)
                                                                    .ThenBy(b => Math.Abs(EF.Functions.DateDiffDay(cutOffDate, b.Month)))
                                                                    .FirstOrDefault(),
                                                  Total = c.Transactions.Where(t => t.DateTime.Year == month.Year && t.DateTime.Month == month.Month)
                                                                        .Sum(t => t.Amount)
                                              }),
                          ExpenseCategories = g.Where(c => c.CategoryType == 2)
                                               .Select(c => new CategoryDTO
                                              {
                                                  Id = c.Id,
                                                  CategoryType = c.CategoryType,
                                                  Name = c.Name,
                                                  Budget = c.Budget,
                                                  BudgetLimit = c.PreviousBudgets
                                                                    .Select(c => new BudgetLimit
                                                                    {
                                                                        Budget = c.Budget,
                                                                        Month = c.Month
                                                                    })
                                                                    .OrderBy(b => b.Month >= cutOffDate)
                                                                    .ThenBy(b => Math.Abs(EF.Functions.DateDiffDay(cutOffDate, b.Month)))
                                                                    .FirstOrDefault(),
                                                  Total = c.Transactions.Where(t => t.DateTime.Year == month.Year && t.DateTime.Month == month.Month)
                                                                        .Sum(t => t.Amount),
                                                  HappyTotal = c.Transactions.Where(t => t.DateTime.Year == month.Year && t.DateTime.Month == month.Month)
                                                                             .Where(t => t.PreviousIsHappy && t.Evaluated || !t.Evaluated && t.IsHappy)
                                                                             .Sum(t => t.Amount),
                                                  NecessaryTotal = c.Transactions.Where(t => t.DateTime.Year == month.Year && t.DateTime.Month == month.Month)
                                                                                 .Where(t => t.PreviousIsNecessary && t.Evaluated || !t.Evaluated && t.IsNecessary)
                                                                                 .Sum(t => t.Amount),
                                              })
                      });

        return await query.AsNoTracking().SingleOrDefaultAsync();
    }

    public async Task<FiscalPlanByYear?> GetDataByYear(int id, int year)
    {
        var query = _dbSet.Where(c => c.FiscalPlanId == id)
                      .Include(c => c.Transactions.Where(t => t.DateTime.Year == year))
                      .Include(c => c.PreviousBudgets.Where(t => t.Month.Year <= year))
                      .GroupBy(c => 1)
                      .Select(g => new FiscalPlanByYear
                      {
                          Id = id,
                          CategoryStatistics = g.Where(c => c.CategoryType == 2).Select(c => new CategoryStatistic
                          {
                              Category = c.Name,
                              CategoryType = c.CategoryType,
                              Budget = c.Budget,
                              Statistics = c.Transactions.Where(t => t.DateTime.Year == year).GroupBy(t => t.DateTime.Month)
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
                              BudgetLimits = c.PreviousBudgets.OrderBy(bl => bl.Month).Select(s => new BudgetLimit { Budget = s.Budget, Month = s.Month })
                          })
                      });

        return await query.AsNoTracking().SingleOrDefaultAsync();
    }

    public Task<Category?> GetCategoryWithFilteredStatistics(int id, Expression<Func<Category, IEnumerable<CategoryBudget>>> filter)
    {
        return _dbSet.Include(filter)
                     .SingleOrDefaultAsync(c => c.Id == id);
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
