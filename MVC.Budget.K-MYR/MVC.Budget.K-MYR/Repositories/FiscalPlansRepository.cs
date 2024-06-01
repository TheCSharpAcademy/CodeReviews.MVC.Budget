using Microsoft.EntityFrameworkCore;
using MVC.Budget.K_MYR.Data;
using MVC.Budget.K_MYR.Models;

namespace MVC.Budget.K_MYR.Repositories;

public class FiscalPlansRepository : GenericRepository<FiscalPlan>, IFiscalPlansRepository
{
    public FiscalPlansRepository(DatabaseContext context) : base(context)
    { }

    public async Task<FiscalPlanByYear?> GetDataByYear(int id, int year)
    {
        var query = _dbSet.Where(f => f.Id == id)
                     .Include(f => f.ExpenseCategories).ThenInclude(c => c.Transactions.Where(t => t.DateTime.Year == year))
                     .Include(f => f.ExpenseCategories).ThenInclude(c => c.PreviousBudgets.Where(t => t.Month.Year == year))
                     .Select(f => new FiscalPlanByYear
                     {                        
                         Id = f.Id,
                         CategoryStatistics = f.ExpenseCategories.Select(c => new CategoryStatistic
                         {
                             Category = c.Name,
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
                             BudgetLimits = c.PreviousBudgets.Select(s => new BudgetLimit { Budget = s.Budget, Month = s.Month })
                         })
                     });

        return await query.AsNoTracking().FirstOrDefaultAsync();
    }
}
