using Microsoft.EntityFrameworkCore;
using MVC.Budget.K_MYR.Data;
using MVC.Budget.K_MYR.Models;

namespace MVC.Budget.K_MYR.Repositories;

public class FiscalPlansRepository : GenericRepository<FiscalPlan>, IFiscalPlansRepository
{
    public FiscalPlansRepository(DatabaseContext context) : base(context)
    { }

    public Task<List<FiscalPlanDTO>> GetAllWithMonthlyData(DateTime month)
    {
        DateTime lowerLimit = new(month.Year, month.Month, 1);
        DateTime upperLimit = lowerLimit.AddMonths(1);

        var query = _dbSet.Select(f => new FiscalPlanDTO
        {
            Id = f.Id,
            Name = f.Name,
            TotalIncome = f.IncomeCategories.SelectMany(c => c.Transactions.Where(t => t.DateTime >= lowerLimit && t.DateTime < upperLimit))
                                                        .Sum(t => t.Amount),
            BudgetIncome = f.IncomeCategories.Sum(c => c.Budget),
            TotalExpenses = f.ExpenseCategories.SelectMany(c => c.Transactions.Where(t => t.DateTime >= lowerLimit && t.DateTime < upperLimit))
                                                        .Sum(t => t.Amount),
            BudgetExpenses = f.ExpenseCategories.Sum(c => c.Budget)
        });

        return query.AsNoTracking()
                    .AsSplitQuery()
                    .ToListAsync();
    }
}
