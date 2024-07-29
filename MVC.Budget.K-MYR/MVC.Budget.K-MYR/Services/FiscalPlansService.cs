using MVC.Budget.K_MYR.Data;
using MVC.Budget.K_MYR.Models;

namespace MVC.Budget.K_MYR.Services;

public class FiscalPlansService : IFiscalPlansService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<FiscalPlansService> _logger;

    public FiscalPlansService(IUnitOfWork unitOfWork, ILogger<FiscalPlansService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public Task<List<FiscalPlan>> GetFiscalPlans()
    {
        return _unitOfWork.FiscalPlansRepository.GetAsync();
    }

    public ValueTask<FiscalPlan?> GetByIDAsync(int id)
    {
        return _unitOfWork.FiscalPlansRepository.GetByIDAsync(id);
    }

    public FiscalPlan? GetByID(int id)
    {
        return _unitOfWork.FiscalPlansRepository.GetByID(id);
    }

    public async Task<FiscalPlanMonthDTO> GetDataByMonth(FiscalPlan fiscalPlan, DateTime Month)
    {
        var categorieData = await _unitOfWork.CategoriesRepository.GetDataByMonth(fiscalPlan.Id, Month);
        var incomeCategories = categorieData.Where(c => c.CategoryType == 1);
        var expenseCategories = categorieData.Where(c => c.CategoryType == 2);

        return new FiscalPlanMonthDTO
        {
            Month = Month,
            Name = fiscalPlan.Name,
            Id = fiscalPlan.Id,
            IncomeCategories = incomeCategories.OrderBy(c => c.Name),
            ExpenseCategories = expenseCategories.OrderBy(c => c.Name),
            ExpensesTotal = expenseCategories.Sum(c => c.Total),
            ExpensesBudget = expenseCategories.Sum(c => c.BudgetLimit?.Budget ?? c.Budget),
            ExpensesHappyTotal = expenseCategories.Sum(c => c.HappyTotal),
            ExpensesNecessaryTotal = expenseCategories.Sum(c => c.NecessaryTotal),
            IncomeTotal = incomeCategories.Sum(c => c.Total),
            IncomeBudget = incomeCategories.Sum(c => c.BudgetLimit?.Budget ?? c.Budget),
            Overspending = expenseCategories.Sum(c => Math.Max(0, c.Total - (c.BudgetLimit?.Budget ?? c.Budget)))
        };    
    }

    public async Task<FiscalPlanYearDTO> GetDataByYear(int fiscalPlanId, int year)
    {
        var categoryStatistics = await _unitOfWork.CategoriesRepository.GetDataByYear(fiscalPlanId, year);

        var grouped = categoryStatistics.SelectMany(a => a.Statistics).GroupBy(s => s.Month);

        var months = Enumerable.Range(1, 12);

        var stats = new FiscalPlanYearDTO
        {
            TotalPerMonth = months.Select(month => grouped?.FirstOrDefault(g => g.Key == month)?.Sum(a => a.TotalSpent) ?? 0),
            HappyPerMonth = months.Select(month => grouped?.FirstOrDefault(g => g.Key == month)?.Sum(a => a.HappyTransactions) ?? 0),
            HappyEvaluatedPerMonth = months.Select(month => grouped?.FirstOrDefault(g => g.Key == month)?.Sum(a => a.HappyEvaluatedTransactions) ?? 0),
            UnhappyPerMonth = months.Select(month => grouped?.FirstOrDefault(g => g.Key == month)?.Sum(a => a.UnhappyTransactions) ?? 0),
            UnhappyEvaluatedPerMonth = months.Select(month => grouped?.FirstOrDefault(g => g.Key == month)?.Sum(a => a.UnhappyEvaluatedTransactions) ?? 0),
            NecessaryPerMonth = months.Select(month => grouped?.FirstOrDefault(g => g.Key == month)?.Sum(a => a.NecessaryTransactions) ?? 0),
            NecessaryEvaluatedPerMonth = months.Select(month => grouped?.FirstOrDefault(g => g.Key == month)?.Sum(a => a.NecessaryEvaluatedTransactions) ?? 0),
            UnnecessaryPerMonth = months.Select(month => grouped?.FirstOrDefault(g => g.Key == month)?.Sum(a => a.UnnecessaryTransactions) ?? 0),
            UnnecessaryEvaluatedPerMonth = months.Select(month => grouped?.FirstOrDefault(g => g.Key == month)?.Sum(a => a.UnnecessaryEvaluatedTransactions) ?? 0),
            UnevaluatedPerMonth = months.Select(month => grouped?.FirstOrDefault(g => g.Key == month)?.Sum(a => a.UnevaluatedTransactions) ?? 0),
            MonthlyOverspendingPerCategory = categoryStatistics.OrderBy(c => c.Category).Select(c => new MonthlyOverspendingPerCategory
            {
                Category = c.Category,
                OverspendingPerMonth = months.Select(month =>
                    Math.Max(0, (c.Statistics.FirstOrDefault(s => s.Month == month)?.TotalSpent ?? 0)
                    - (c.BudgetLimits.LastOrDefault(bl => bl.Month.Month <= month)?.Budget ?? c.Budget)))
            }) ?? []
        };    

        stats.TotalSpent = stats.TotalPerMonth.Sum();
        stats.OverspendingTotal = stats.MonthlyOverspendingPerCategory.SelectMany(s => s.OverspendingPerMonth).Sum();
        stats.HappyEvaluatedTotal = stats.HappyEvaluatedPerMonth.Sum();
        stats.UnhappyEvaluatedTotal = stats.TotalSpent - stats.HappyEvaluatedTotal;
        stats.NecessaryEvaluatedTotal = stats.NecessaryEvaluatedPerMonth.Sum();
        stats.UnnecessaryEvaluatedTotal = stats.TotalSpent - stats.NecessaryEvaluatedTotal;

        return stats;
    }

    public async Task<FiscalPlan> AddFiscalPlan(FiscalPlanPost fiscaPlanPost)
    {
        var fiscalPlan = new FiscalPlan()
        {
            Name = fiscaPlanPost.Name,
        };

        _unitOfWork.FiscalPlansRepository.Insert(fiscalPlan);

        await _unitOfWork.Save();

        return fiscalPlan;
    }

    public async Task UpdateFiscalPlan(FiscalPlan fiscalPlan, FiscalPlanPut fiscalPlanPut)
    {
        fiscalPlan.Name = fiscalPlanPut.Name;

        await _unitOfWork.Save();
    }

    public async Task DeleteFiscalPlan(FiscalPlan fiscalPlan)
    {
        _unitOfWork.FiscalPlansRepository.Delete(fiscalPlan);
        await _unitOfWork.Save();
    }
}
